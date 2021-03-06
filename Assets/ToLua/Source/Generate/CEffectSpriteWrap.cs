﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class CEffectSpriteWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(CEffectSprite), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("SetPlayEndCallBack", SetPlayEndCallBack);
		L.RegFunction("SetKeyFrameCallBack", SetKeyFrameCallBack);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("m_bEndDestroy", get_m_bEndDestroy, set_m_bEndDestroy);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPlayEndCallBack(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			CEffectSprite obj = (CEffectSprite)ToLua.CheckObject<CEffectSprite>(L, 1);
			OnSkillEnd arg0 = (OnSkillEnd)ToLua.CheckDelegate<OnSkillEnd>(L, 2);
			object arg1 = ToLua.ToVarObject(L, 3);
			obj.SetPlayEndCallBack(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetKeyFrameCallBack(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			CEffectSprite obj = (CEffectSprite)ToLua.CheckObject<CEffectSprite>(L, 1);
			System.Action arg0 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 2);
			obj.SetKeyFrameCallBack(arg0);
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
	static int get_m_bEndDestroy(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			CEffectSprite obj = (CEffectSprite)o;
			bool ret = obj.m_bEndDestroy;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index m_bEndDestroy on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_bEndDestroy(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			CEffectSprite obj = (CEffectSprite)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.m_bEndDestroy = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index m_bEndDestroy on a nil value");
		}
	}
}

