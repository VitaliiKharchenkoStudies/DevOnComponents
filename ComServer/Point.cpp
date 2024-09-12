// Point.cpp : Implementation of CPoint

#include "pch.h"
#include "stdafx.h"
#include "Vector3DServer.h"
#include "Point.h"

STDMETHODIMP CPoint::SetCoordinates(double x, double y, double z)
{
    m_x = x;
    m_y = y;
    m_z = z;
    return S_OK;
}

STDMETHODIMP CPoint::GetCoordinates(double* x, double* y, double* z)
{
    *x = m_x;
    *y = m_y;
    *z = m_z;
    return S_OK;
}
