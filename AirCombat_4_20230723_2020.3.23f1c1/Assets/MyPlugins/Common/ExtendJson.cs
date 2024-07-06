/****************************************************
    文件：ExtendLitJson.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/24 22:32:11
	功能：
*****************************************************/

using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;
using Newtonsoft.Json;


public enum JsonParseType
{ 
    JsonMapper,
    JsonConvert,

}
public static partial class ExtendJson
{
    public static T JsonStr2Object<T>(this string jsonData,JsonParseType type)
    {


        switch ( type )
        {
            case JsonParseType.JsonMapper: return jsonData.JsonStr2Object_JsonMapper<T>();
            default:throw new System.Exception(" JsonStr2Object异常"); break;
        }
    }

    public static T JsonPath2Object<T>(this string jsonPath, JsonParseType type)
    {


        switch (type)
        {
            case JsonParseType.JsonMapper: return jsonPath.JsonPath2Object_JsonMapper<T>();
            case JsonParseType.JsonConvert: return jsonPath.JsonPath2Object_JsonConvert<T>();
            default: throw new System.Exception(" JsonStr2Object异常"); break;
        }
    }

}
public static partial class ExtendJson
{

    #region 内部类(主要是构造方法的区别)

    public class Heros
    {
        public List<Hero> HeroLst = new List<Hero>();

        public override string ToString()
        {
            string str = "";
            foreach (Hero hero in HeroLst)
            {
                str += "\n" + hero.ToString();
            }
            return str;
        }
    }

    /// <summary>
    /// 01 里面的属性字段一定要有public
    /// <para />System.Text.Json.JsonSerializer.Serialize就一定要有publc的属性
    /// </summary>
    public class Hero
    {
        public Hero(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; set; }
        public int Age { get; set; }
        //或者
        //string _name;
        //int _age;
        //public string Name { get => _name; set => _name = value; }
        //public int Age { get => _age; set => _age = value; }


        //public Hero(string name, int age)
        //{
        //    this.Name = name;
        //    this.Age = age;
        //}



        public override string ToString()
        {
            string str = "";
            str += "\t" + Name;
            str += "\t" + Age;

            return str;
        }

    }

    public class Person
    {


        public string Name { get; set; }
        public int Age { get; set; }
        //或者
        //string _name;
        //int _age;
        //public string Name { get => _name; set => _name = value; }
        //public int Age { get => _age; set => _age = value; }




        public override string ToString()
        {
            string str = "";
            str += "\t" + Name;
            str += "\t" + Age;

            return str;
        }


    }

    public class Character
    {
        public Character()
        {

        }
        public Character(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; set; }
        public int Age { get; set; }
        //或者
        //string _name;
        //int _age;
        //public string Name { get => _name; set => _name = value; }
        //public int Age { get => _age; set => _age = value; }




        public override string ToString()
        {
            string str = "";
            str += "\t" + Name;
            str += "\t" + Age;

            return str;
        }


    }


    #endregion


    public static void Example()
    {
        if (false)
        {
            JsonData data = new JsonData();
            JsonType type = data.GetJsonType();
        }
        if (false)
        {
            ExtendJson.Hero hero = new ExtendJson.Hero("刘备", 18);
            Debug.Log(hero.Object2Json());
        }
        if (false)
        {

            ExtendJson.Hero hero1 = new ExtendJson.Hero("刘备", 18);
            ExtendJson.Hero hero2 = new ExtendJson.Hero("关羽", 18);
            ExtendJson.Hero hero3 = new ExtendJson.Hero("张飞", 18);
            ExtendJson.Hero[] heroArr = new ExtendJson.Hero[3] { hero1, hero2, hero3 };
            Debug.Log(heroArr.Object2Json());
        }
        if (false)
        {
            ExtendJson.Hero hero1 = new ExtendJson.Hero("刘备", 18);
            ExtendJson.Hero hero2 = new ExtendJson.Hero("关羽", 18);
            ExtendJson.Hero hero3 = new ExtendJson.Hero("张飞", 18);
            ExtendJson.Heros heros = new ExtendJson.Heros();
            heros.HeroLst.Add(hero1);
            heros.HeroLst.Add(hero2);
            heros.HeroLst.Add(hero3);
            Debug.Log(heros.Object2Json());

        }
        if (false) //想采用 System.Text.Json.JsonSerializer.Serialize。但是引用不到
        {
            //string str1 = @"{'Name':'刘备','Age':18}";//@+'
            //string str2 = "  /{/"Name/":/"刘备/",/"Age/":18/}  ";
            var stream = new ExtendJson.Person { Name ="刘备",Age=18 };
            // string str = System.Text.Json.JsonSerializer.Serialize(stream);
            string str = "";
            //
            ExtendJson.Person person;
            person = str.JsonStr2Object<ExtendJson.Person>();
            Debug.Log(person.ToString());
        }
        if (false)
        {
            ExtendJson.Character cPre=new ExtendJson.Character("刘备",18);
            string json = cPre.Object2Json() ;
            Debug.Log("01 "+json);
            ExtendJson.Character cAfter = json.JsonStr2Object<ExtendJson.Character>();
            Debug.Log("02 " + cAfter.ToString());
        }
        if(true)
        {
            //里面是{"Name":"刘备","Age":18}
            string str = File.ReadAllText(Application.streamingAssetsPath+"/Config/Hero.json");
            Debug.Log(str);
            Character hero=  str.JsonStr2Object<Character>();
            Debug.Log(hero.ToString());
        }
    }

    public static JsonType GetJsonType_Common(this JsonData data)
    {
        return data.GetJsonType();
    }

    /// <summary>
    /// Newtonsoft.Json
    /// <para />File.ReadAllText
    /// </summary> 
     static T JsonPath2Object_JsonConvert<T>(this string path)
    {
        string jsonStr = File.ReadAllText(path);
        //Debug.Log("JsonPath2Object_JsonConvert\n" + jsonStr);

        T t = JsonConvert.DeserializeObject<T>(jsonStr);
        //Debug.Log("JsonPath2Object_JsonConvert\n" + t.ToString());
        return t;
    }

  
    #region 存档


    #endregion


}

public static partial class ExtendJson  //JsonMapper
{
    /// <summary>
    /// https://blog.csdn.net/weixin_39562801/article/details/90410402
    /// </summary> 
    public static string Object2Json<T>(this T t)
    {
        string str = JsonMapper.ToJson(t);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        str = reg.Replace(str, delegate (Match m)
        {
            return ((char)System.Convert.ToInt32(m.Groups[1].Value, 16)).ToString();
        });

        return str;
    }



    /// <summary>
    /// 01 File.ReadText(_path)
    /// 需要一个无参构造函数
    /// </summary>
    public static T JsonStr2Object<T>(this string jsonStr)
    {
        T t = JsonMapper.ToObject<T>(jsonStr);
        return t;
    }

    /// <summary>
    /// LitJson
    /// <para />File.ReadAllText
    /// </summary> 
     static T JsonPath2Object_JsonMapper<T>(this string path)
    {
        string str = File.ReadAllText(path);
        //Debug.Log("JsonPath2Object_JsonMapper\n" + str);

        T t = JsonMapper.ToObject<T>(str);
        //  Debug.Log("JsonPath2Object_JsonMapper\n" + t.ToString());
        return t;
    }

     static T JsonStr2Object_JsonMapper<T>(this string jsonData)
    {
        // Debug.Log("JsonData2Object_JsonMapper\n" + jsonData);

        T t = JsonMapper.ToObject<T>(jsonData);
        //  Debug.Log("JsonData2Object_JsonMapper\n" + t.ToString());
        return t;
    }

    public static T JsonData2Object_JsonMapper<T>(this JsonData jsonData)
    {
        string str = jsonData.ToJson();
        T t = JsonMapper.ToObject<T>(str);
        return t;
    }

    public static JsonData JsonPath2JsonData_JsonMapper(this string path)
    {
        string str = File.ReadAllText(path);
        //   Debug.Log("JsonPath2JsonData_JsonMapper\n" + str);

        JsonData jsonData = JsonMapper.ToObject(str);
        //  Debug.Log("JsonPath2JsonData_JsonMapper\n" + jsonData.ToString());
        return jsonData;
    }

    public static JsonData JsonStr2JsonData_JsonMapper(this string str)
    {
        JsonData jsonData = JsonMapper.ToObject(str);
        //  Debug.Log("JsonPath2JsonData_JsonMapper\n" + jsonData.ToString());
        return jsonData;
    }

    public static void Object2JsonFile_JsonMapper(this object obj, string path)
    {
        string json = JsonMapper.ToJson(obj);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="path"> Application.streamingAssetsPath/xxx.json这种格式</param>
    public static void Object2JsonFile_SW_JsonMapper<T>(T obj, string path)
    {
        string str = JsonMapper.ToJson(obj);
        StreamWriter streamWriter = new StreamWriter(path);
        streamWriter.Write(str);
        streamWriter.Close();
    }
}




