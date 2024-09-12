// Vector.cpp : Implementation of CVector

#include "pch.h"
#include "stdafx.h"
#include "Vector3DServer.h"
#include "Vector.h"

STDMETHODIMP CVector::SetVector(double x, double y, double z)
{
    m_x = x;
    m_y = y;
    m_z = z;
    return S_OK;
}

STDMETHODIMP CVector::GetVector(double* x, double* y, double* z)
{
    *x = m_x;
    *y = m_y;
    *z = m_z;
    return S_OK;
}

STDMETHODIMP CVector::GetUnitVector(double* x, double* y, double* z)
{
    double length = sqrt(m_x * m_x + m_y * m_y + m_z * m_z);
    *x = m_x / length;
    *y = m_y / length;
    *z = m_z / length;
    return S_OK;
}

STDMETHODIMP CVector::FromString(BSTR data)
{
    // Example: "1.0 2.0 3.0"
    swscanf_s(data, L"%lf %lf %lf", &m_x, &m_y, &m_z);
    return S_OK;
}

STDMETHODIMP CVector::ToString(BSTR* data)
{
    wchar_t buffer[100];
    swprintf_s(buffer, L"%lf %lf %lf", m_x, m_y, m_z);
    *data = SysAllocString(buffer);
    return S_OK;
}
