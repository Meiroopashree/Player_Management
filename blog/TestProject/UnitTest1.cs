using System.Numerics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Microsoft.Data.SqlClient;
using System;

namespace TestProject
{

public class Tests
{
        [Test]
        public void Post_Model_ClassExists()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Model.Post";
            Assembly assembly = Assembly.Load(assemblyName);
            Type postType = assembly.GetType(typeName);
            Assert.IsNotNull(postType);
        }

        [Test]
        public void Post_Id_PropertyExists_ReturnExpectedDataTypes_int()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Model.Post";
            Assembly assembly = Assembly.Load(assemblyName);
            Type PostType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = PostType.GetProperty("Id");
            Assert.IsNotNull(propertyInfo, "Property Id does not exist in Post class");
            Type expectedType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(int), expectedType, "Property Id in Post class is not of type int");
        }

        [Test]
        public void Post_Title_PropertyExists_ReturnExpectedDataTypes_string()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Model.Post";
            Assembly assembly = Assembly.Load(assemblyName);
            Type PostType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = PostType.GetProperty("Title");
            Assert.IsNotNull(propertyInfo, "Property Title does not exist in Post class");
            Type expectedType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(string), expectedType, "Property Title in Post class is not of type string");
        }

        [Test]
        public void Post_Content_PropertyExists_ReturnExpectedDataTypes_string()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Model.Post";
            Assembly assembly = Assembly.Load(assemblyName);
            Type PostType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = PostType.GetProperty("Content");
            Assert.IsNotNull(propertyInfo, "Property Content does not exist in Post class");
            Type expectedType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(string), expectedType, "Property Content in Post class is not of type string");
        }

        [Test]
        public void Comment_Model_ClassExists()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Model.Comment";
            Assembly assembly = Assembly.Load(assemblyName);
            Type CommentType = assembly.GetType(typeName);
            Assert.IsNotNull(CommentType);
        }

        [Test]
        public void Comment_Id_PropertyExists_ReturnExpectedDataTypes_int()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Model.Comment";
            Assembly assembly = Assembly.Load(assemblyName);
            Type CommentType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = CommentType.GetProperty("Id");
            Assert.IsNotNull(propertyInfo, "Property Id does not exist in Comment class");
            Type expectedType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(int), expectedType, "Property Id in Post class is not of type int");
        }

        [Test]
        public void Comment_Text_PropertyExists_ReturnExpectedDataTypes_string()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Model.Comment";
            Assembly assembly = Assembly.Load(assemblyName);
            Type CommentType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = CommentType.GetProperty("Text");
            Assert.IsNotNull(propertyInfo, "Property Text does not exist in Comment class");
            Type expectedType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(string), expectedType, "Property Text in Comment class is not of type string");
        }

        [Test]
        public void PostController_Controllers_ClassExists()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Controllers.PostController";
            Assembly assembly = Assembly.Load(assemblyName);
            Type PostControllerType = assembly.GetType(typeName);
            Assert.IsNotNull(PostControllerType);
        }

        [Test]
        public void CommentController_Controllers_ClassExists()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Controllers.CommentController";
            Assembly assembly = Assembly.Load(assemblyName);
            Type CommentControllerType = assembly.GetType(typeName);
            Assert.IsNotNull(CommentControllerType);
        }

        [Test]
        public void PostController_AddPost_MethodExists()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Controllers.PostController";
            Assembly assembly = Assembly.Load(assemblyName);
            Type PostControllerType = assembly.GetType(typeName);
            MethodInfo methodInfo = PostControllerType.GetMethod("AddPost", Type.EmptyTypes);
            Assert.IsNotNull(methodInfo, "Method AddPost does not exist in PostController class");
        }


}
}