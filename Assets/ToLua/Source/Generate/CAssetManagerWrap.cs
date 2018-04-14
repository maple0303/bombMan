﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class CAssetManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(CAssetManager), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("GetAsset", GetAsset);
		L.RegFunction("UnloadAsset", UnloadAsset);
		L.RegFunction("GetAssetSprite", GetAssetSprite);
		L.RegFunction("GetAssetSpriteAsync", GetAssetSpriteAsync);
		L.RegFunction("LoadResAsync", LoadResAsync);
		L.RegFunction("ExeCoroutineTask", ExeCoroutineTask);
		L.RegFunction("ClearAsyncLoadingTask", ClearAsyncLoadingTask);
		L.RegFunction("StopCurAsyncLoading", StopCurAsyncLoading);
		L.RegFunction("DoCoroutineTask", DoCoroutineTask);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAsset(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityEngine.Object o = CAssetManager.GetAsset(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 2)
			{
				string arg0 = ToLua.CheckString(L, 1);
				System.Type arg1 = ToLua.CheckMonoType(L, 2);
				UnityEngine.Object o = CAssetManager.GetAsset(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: CAssetManager.GetAsset");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadAsset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			CAssetManager.UnloadAsset(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAssetSprite(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityEngine.Sprite o = CAssetManager.GetAssetSprite(arg0);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else if (count == 2)
			{
				string arg0 = ToLua.CheckString(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				UnityEngine.Sprite o = CAssetManager.GetAssetSprite(arg0, arg1);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: CAssetManager.GetAssetSprite");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAssetSpriteAsync(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				CAssetManager obj = (CAssetManager)ToLua.CheckObject<CAssetManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				obj.GetAssetSpriteAsync(arg0);
				return 0;
			}
			else if (count == 3)
			{
				CAssetManager obj = (CAssetManager)ToLua.CheckObject<CAssetManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				string arg1 = ToLua.CheckString(L, 3);
				obj.GetAssetSpriteAsync(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				CAssetManager obj = (CAssetManager)ToLua.CheckObject<CAssetManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				string arg1 = ToLua.CheckString(L, 3);
				System.Action<UnityEngine.Object> arg2 = (System.Action<UnityEngine.Object>)ToLua.CheckDelegate<System.Action<UnityEngine.Object>>(L, 4);
				obj.GetAssetSpriteAsync(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: CAssetManager.GetAssetSpriteAsync");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadResAsync(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			CAssetManager obj = (CAssetManager)ToLua.CheckObject<CAssetManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			System.Action<UnityEngine.Object> arg1 = (System.Action<UnityEngine.Object>)ToLua.CheckDelegate<System.Action<UnityEngine.Object>>(L, 3);
			bool arg2 = LuaDLL.luaL_checkboolean(L, 4);
			obj.LoadResAsync(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ExeCoroutineTask(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			CAssetManager obj = (CAssetManager)ToLua.CheckObject<CAssetManager>(L, 1);
			System.Action arg0 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 2);
			System.Action<object> arg1 = (System.Action<object>)ToLua.CheckDelegate<System.Action<object>>(L, 3);
			object arg2 = ToLua.ToVarObject(L, 4);
			obj.ExeCoroutineTask(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearAsyncLoadingTask(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			CAssetManager obj = (CAssetManager)ToLua.CheckObject<CAssetManager>(L, 1);
			obj.ClearAsyncLoadingTask();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StopCurAsyncLoading(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			CAssetManager obj = (CAssetManager)ToLua.CheckObject<CAssetManager>(L, 1);
			obj.StopCurAsyncLoading();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DoCoroutineTask(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			CAssetManager obj = (CAssetManager)ToLua.CheckObject<CAssetManager>(L, 1);
			System.Action arg0 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 2);
			System.Action<object> arg1 = (System.Action<object>)ToLua.CheckDelegate<System.Action<object>>(L, 3);
			object arg2 = ToLua.ToVarObject(L, 4);
			System.Collections.IEnumerator o = obj.DoCoroutineTask(arg0, arg1, arg2);
			ToLua.Push(L, o);
			return 1;
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
}
