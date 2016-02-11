// ActionUtils.cs
//
//  Author:
//       Roman Feshchenko <roman.feshchenko@meliorgames.com>
//  Created:
//      09/25/2015
//
//  Copyright (c) 2015 
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;

public static class ActionUtils
{
	public static void SafeInvoke(this Action invocationTarget)
	{
		if(null != invocationTarget)
		{
			invocationTarget.Invoke();
		}
	}

	public static void SafeInvoke<T>(this Action<T> invocationTarget, T arg)
	{
		if(null != invocationTarget)
		{
			invocationTarget.Invoke(arg);
		}
	}

	public static void SafeInvoke<T1, T2>(this Action<T1, T2> invocationTarget, T1 arg1, T2 arg2)
	{
		if(null != invocationTarget)
		{
			invocationTarget.Invoke(arg1, arg2);
		}
	}

	public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> invocationTarget, T1 arg1, T2 arg2, T3 arg3)
	{
		if(null != invocationTarget)
		{
			invocationTarget.Invoke(arg1, arg2, arg3);
		}
	}
}