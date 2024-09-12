// dllmain.h : Declaration of module class.

class CComServerModule : public ATL::CAtlDllModuleT< CComServerModule >
{
public :
	DECLARE_LIBID(LIBID_ComServerLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_COMSERVER, "{dba264c4-2fe2-4d7b-ad37-452721bd2910}")
};

extern class CComServerModule _AtlModule;
