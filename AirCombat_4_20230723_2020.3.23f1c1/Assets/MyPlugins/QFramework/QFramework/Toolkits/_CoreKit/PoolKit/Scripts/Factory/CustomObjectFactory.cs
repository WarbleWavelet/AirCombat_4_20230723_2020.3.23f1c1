/****************************************************************************
 * Copyright (c) 2015 - 2022 liangxiegame UNDER MIT License
 * 
 * http://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using System;

namespace QFramework
{
    /// <summary>
    /// 自定义对象工厂：相关对象是 自己定义 
    /// <para/> CustomObjectFactory(Func&lt;T&gt; factoryMethod)
    /// <br/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomObjectFactory<T> : IObjectFactory<T>
    {          
        protected Func<T> mFactoryMethod;


        public CustomObjectFactory(Func<T> factoryMethod)
        {
            mFactoryMethod = factoryMethod;
        }

        public T Create()
        {
            return mFactoryMethod();
        }
    }
}