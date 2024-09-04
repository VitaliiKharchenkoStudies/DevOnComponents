#include <iostream>
#include <comdef.h>
#include <atlbase.h>
#include <vector>
#include <string>

#import "CSCOMServer.tlb" raw_interfaces_only

using namespace CSCOMServer;
using namespace std;


int main()
{
    // Initialize COM
    HRESULT hr = CoInitialize(nullptr);
    if (FAILED(hr))
    {
        cerr << "Failed to initialize COM library." << endl;
        return 1;
    }

    {
        // Create instances of the COM objects
        IPointPtr firstPoint;
        IPointPtr secondPoint;
        IPointPtr thirdPoint;
        ICircumferencePtr initialCircle;
        
        hr = firstPoint.CreateInstance(__uuidof(Point));
        hr = secondPoint.CreateInstance(__uuidof(Point));
        hr = thirdPoint.CreateInstance(__uuidof(Point));
        hr = initialCircle.CreateInstance(__uuidof(Circle));

        if (FAILED(hr))
        {
            cerr << "Failed to create COM objects." << endl;
            CoUninitialize();
            return 1;
        }

        // Enter and parse initial data
        ProcessData(firstPoint, secondPoint, thirdPoint, initialCircle);

        // Find all points that are inside the circle
        vector<IPointPtr> pointsInsideCircle;
        if (initialCircle != nullptr)
        {
            for (IPointPtr point : { firstPoint, secondPoint, thirdPoint })
            {
                VARIANT_BOOL contains;
                hr = initialCircle->Contains(point, &contains);
                if (SUCCEEDED(hr) && contains == VARIANT_TRUE)
                {
                    pointsInsideCircle.push_back(point);
                }
            }
        }

        // Get information about the second circle
        long newRadius = 0.0;
        if (firstPoint != nullptr && secondPoint != nullptr)
        {
            hr = firstPoint->DistanceTo(secondPoint, &newRadius);
        }

        ICircumferencePtr secondCircle;
        if (firstPoint != nullptr)
        {
            hr = secondCircle.CreateInstance(__uuidof(Circle));
            if (SUCCEEDED(hr) && secondCircle != nullptr)
            {
                secondCircle->putref_Center(firstPoint);
                secondCircle->put_Radius(newRadius);
            }
        }

        // Output results
        if (!pointsInsideCircle.empty() && secondCircle != nullptr)
        {
            cout << "All points that are inside the initial circle: ";
            for (size_t i = 0; i < pointsInsideCircle.size(); ++i)
            {
                double x, y;
                pointsInsideCircle[i]->get_X(&x);
                pointsInsideCircle[i]->get_Y(&y);
                cout << "(" << x << ";" << y << ")";
                if (i < pointsInsideCircle.size() - 1)
                    cout << ",";
            }
            cout << endl;

            double center_x, center_y;
            IPointPtr centerPoint;
            hr = secondCircle->get_Center(&centerPoint);
            centerPoint->get_X(&center_x);
            centerPoint->get_Y(&center_y);

            cout << "Information about the circle whose center coincides with the first point entered and the circle itself passes through the second point entered: ";
            cout << "Center: (" << center_x << ";" << center_y << "), Radius: " << newRadius << endl;
        }
    }

    // Uninitialize COM
    CoUninitialize();
    return 0;
}

void ProcessData(IPointPtr firstPoint, IPointPtr secondPoint, IPointPtr thirdPoint, ICircumferencePtr circle)
{
    cout << "Enter data for 3 points and 1 circle (format: x1;y1 x2;y2 x3;y3 x4;y4;r):" << endl;
    std::string data;
    std::getline(std::cin, data);

    // Parse data
    BSTR bstrData = SysAllocStringLen(NULL, data.length());
    MultiByteToWideChar(CP_ACP, 0, data.c_str(), -1, bstrData, data.length());

    // Split data
    SAFEARRAY* psa = SafeArrayCreateVector(VT_BSTR, 0, 4);
    LONG i;
    BSTR str = bstrData;
    for (i = 0; i < 4; i++)
    {
        SafeArrayPutElement(psa, &i, str);
        str += wcslen(str) + 1;
    }

    // Read data for points
    
    firstPoint->ReadData(SysAllocStringByteLen(data.c_str(), data.size()));
    secondPoint->ReadData(SysAllocStringByteLen(data.c_str(), data.size()));
    thirdPoint->ReadData(SysAllocStringByteLen(data.c_str(), data.size()));

    // Read data for circle
    circle->ReadData(SysAllocStringByteLen(data.c_str(), data.size()));
}