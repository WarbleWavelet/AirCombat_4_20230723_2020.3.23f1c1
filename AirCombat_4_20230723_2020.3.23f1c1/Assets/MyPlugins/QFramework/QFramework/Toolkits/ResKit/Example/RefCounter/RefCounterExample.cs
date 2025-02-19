﻿/****************************************************************************
 * Copyright (c) 2018.6 liangxie
 * 
 * http://qframework.io
 * https://github.com/liangxiegame/QFramework
 * 
 * Permission is hereby granted, free of charge, _to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), _to deal
 * in the Software without restriction, including without limitation the rights
 * _to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and _to permit persons _to whom the Software is
 * furnished _to do so, subject _to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR PoolObjectBase PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using UnityEngine;

namespace QFramework.Course
{
    public class RefCounterExample : MonoBehaviour
    {
        void Start()
        {
            var room = new Room();

            room.EnterPeople();
            room.EnterPeople();
            room.EnterPeople();

            room.LeavePeople();
            room.LeavePeople();
            room.LeavePeople();
        }
    }

    public class Light
    {
        public void SwitchOn()
        {
            Debug.Log("开灯");
        }

        public void SwitchOff()
        {
            Debug.Log("关灯");
        }
    }

    public class Room : SimpleRC
    {
        private Light mLight = new Light();

        public void EnterPeople()
        {
            Debug.LogError("进入人了");

            if (RefCount == 0)
            {
                mLight.SwitchOn();
            }

            Retain();
        }

        public void LeavePeople()
        {
            Release();

            Debug.LogError("人出来了");
        }

        protected override void OnZeroRef()
        {
            mLight.SwitchOff();
        }
    }
}