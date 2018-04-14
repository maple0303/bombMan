﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class CHeroControllerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(CHeroController), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("SetRoleMoveIngCallBack", SetRoleMoveIngCallBack);
		L.RegFunction("SetClickGroundCallBack", SetClickGroundCallBack);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("m_controllerTarget", get_m_controllerTarget, set_m_controllerTarget);
		L.RegVar("m_unSelectEntityID", get_m_unSelectEntityID, set_m_unSelectEntityID);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetRoleMoveIngCallBack(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			CHeroController obj = (CHeroController)ToLua.CheckObject<CHeroController>(L, 1);
			OnHeroClickEntity arg0 = (OnHeroClickEntity)ToLua.CheckDelegate<OnHeroClickEntity>(L, 2);
			obj.SetRoleMoveIngCallBack(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetClickGroundCallBack(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			CHeroController obj = (CHeroController)ToLua.CheckObject<CHeroController>(L, 1);
			OnHeroClickGround arg0 = (OnHeroClickGround)ToLua.CheckDelegate<OnHeroClickGround>(L, 2);
			obj.SetClickGroundCallBack(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_controllerTarget(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			CHeroController obj = (CHeroController)o;
			CRoleSprite ret = obj.m_controllerTarget;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index m_controllerTarget on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_unSelectEntityID(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			CHeroController obj = (CHeroController)o;
			uint ret = obj.m_unSelectEntityID;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index m_unSelectEntityID on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_controllerTarget(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			CHeroController obj = (CHeroController)o;
			CRoleSprite arg0 = (CRoleSprite)ToLua.CheckObject<CRoleSprite>(L, 2);
			obj.m_controllerTarget = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index m_controllerTarget on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_unSelectEntityID(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			CHeroController obj = (CHeroController)o;
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 2);
			obj.m_unSelectEntityID = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index m_unSelectEntityID on a nil value");
		}
	}
}

