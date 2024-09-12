#include <iostream>
#include <string>  
#include <Windows.h>
#include "COMServer_h.h"  


void InitializeCOM()
{
    HRESULT hr = CoInitialize(NULL);
    if (FAILED(hr))
    {
        std::cout << "COM initialization failed!" << std::endl;
        exit(1);
    }
}

void UninitializeCOM()
{
    CoUninitialize();
}

int main()
{
    InitializeCOM();

    I3DPoint* point = NULL;
    I3DVector* vector = NULL;

    // Create COM objects
    HRESULT hr = CoCreateInstance(CLSID_Point3D, NULL, CLSCTX_INPROC_SERVER, IID_I3DPoint, (void**)&point);
    if (FAILED(hr))
    {
        std::cout << "Failed to create Point3D COM object!" << std::endl;
        return 1;
    }

    hr = CoCreateInstance(CLSID_Vector3D, NULL, CLSCTX_INPROC_SERVER, IID_I3DVector, (void**)&vector);
    if (FAILED(hr))
    {
        std::cout << "Failed to create Vector3D COM object!" << std::endl;
        return 1;
    }

    // Read input from user
    std::cout << "Enter point (x;y;z) and vector (vx;vy;vz) as a single line: ";
    std::string input;
    std::getline(std::cin, input);

    // Split input into point and vector parts
    size_t semiColonPos = input.find(';');
    std::string pointData = input.substr(0, semiColonPos);
    std::string vectorData = input.substr(semiColonPos + 1);

    // Read data into the COM objects
    point->ReadData(std::wstring(pointData.begin(), pointData.end()).c_str());
    vector->ReadData(std::wstring(vectorData.begin(), vectorData.end()).c_str());

    // Get vector magnitude to normalize it
    double length = 0.0;
    vector->Length(&length);

    // Normalize the vector
    I3DVector* unitVector = NULL;
    hr = CoCreateInstance(CLSID_Vector3D, NULL, CLSCTX_INPROC_SERVER, IID_I3DVector, (void**)&unitVector);
    vector->Normalize(&unitVector);

    // Translate point by twice the length of the vector
    I3DPoint* newPoint = NULL;
    point->Translate(vector, 2, &newPoint); // Move point along vector scaled by 2

    // Output the normalized vector and new point
    BSTR result;
    unitVector->ToString(&result);
    std::wcout << "Unit vector: " << result << std::endl;
    SysFreeString(result);

    newPoint->ToString(&result);
    std::wcout << "New point: " << result << std::endl;
    SysFreeString(result);

    // Cleanup
    point->Release();
    vector->Release();
    unitVector->Release();
    newPoint->Release();
    UninitializeCOM();

    return 0;
}
