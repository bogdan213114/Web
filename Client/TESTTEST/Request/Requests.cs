using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TESTTEST.MVVM.Model;
using TESTTEST.Response;

namespace TESTTEST.Request
{
    public class Requests
    {
        public static async Task<AuthenticationResponse> AuthentificateAsync(string Login,string Passwd)
        {
            
            string payload = JsonConvert.SerializeObject(new
            {
                name = Login,
                passwd = Passwd,
            });
            HttpClient client = new HttpClient();
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(@"http://localhost:44302/api/auth/login", content);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            AuthenticationResponse JwtHolder = JsonConvert.DeserializeObject<AuthenticationResponse>(responseContent);
            return JwtHolder;
  
        }
        public static async Task<User> Get_User(AuthenticationResponse JwtHolder)
        {

            AuthenticationResponse JwtHolder1 = await GetRefresh(JwtHolder);
            if (JwtHolder1 != null)
            {
                JwtHolder.State = JwtHolder1.State;
                JwtHolder.JWT = JwtHolder1.JWT;
                JwtHolder.RT = JwtHolder1.RT;
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("jwt", JwtHolder.JWT.Value.Value);
            var GetRequest = new HttpRequestMessage(HttpMethod.Get, @"http://localhost:44302/api/getUser");
            var getresponse = await client.SendAsync(GetRequest);
            var getresponseTake = getresponse.Content.ReadAsStringAsync().Result;
            GetUserResponseBody UserandState = JsonConvert.DeserializeObject<GetUserResponseBody>(getresponseTake);
            User user = UserandState.data;
            return user;
        }
        public static async Task<AuthenticationResponse> RegistrateAsync(string Login, string Passwd)
        {

            string payload = JsonConvert.SerializeObject(new
            {
                name = Login,
                passwd = Passwd,
            });
            HttpClient client = new HttpClient();
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
          
            var response = await client.PostAsync(@"http://localhost:44302/api/auth/register", content);
          
            var responseContent = response.Content.ReadAsStringAsync().Result;
            AuthenticationResponse JwtHolder = JsonConvert.DeserializeObject<AuthenticationResponse>(responseContent);
            return JwtHolder;
        }

        public static async Task<ToDoTask> AddTaskAsync(ToDoTask todotask, AuthenticationResponse JwtHolder)
        {

            AuthenticationResponse JwtHolder1 = await GetRefresh(JwtHolder);
            if (JwtHolder1 != null)
            {
                JwtHolder.State = JwtHolder1.State;
                JwtHolder.JWT = JwtHolder1.JWT;
                JwtHolder.RT = JwtHolder1.RT;
            }
            string payload = JsonConvert.SerializeObject(new
            {
                title = todotask.Title,
                description = todotask.Description,
                isDone = todotask.IsDone,
            });
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("jwt", JwtHolder.JWT.Value.Value);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(@"http://localhost:44302/api/createTask", content);
            
            var responseContent = response.Content.ReadAsStringAsync().Result;
           
            ToDoTaskAddResponse IdHolder = JsonConvert.DeserializeObject<ToDoTaskAddResponse>(responseContent);
          
            todotask.Id = IdHolder.entryid;
            return todotask;
        }
        public static async Task<UserDataGettingState> DeleteTaskAsync(ToDoTask todotask, AuthenticationResponse JwtHolder)
        {

            AuthenticationResponse JwtHolder1 = await GetRefresh(JwtHolder);
            if (JwtHolder1 != null)
            {
                JwtHolder.State = JwtHolder1.State;
                JwtHolder.JWT = JwtHolder1.JWT;
                JwtHolder.RT = JwtHolder1.RT;
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("jwt", JwtHolder.JWT.Value.Value);
            var response = await client.DeleteAsync($"http://localhost:44302/api/deleteTask/{todotask.Id}");
            var responseContent = response.Content.ReadAsStringAsync().Result;
            ToDoTaskDeleteResponse StatusHolder = JsonConvert.DeserializeObject<ToDoTaskDeleteResponse>(responseContent);
            return StatusHolder.state;
        }
        public static async Task<UserDataGettingState> EditTaskAsync(ToDoTask selectedtask, ToDoTask todotask, AuthenticationResponse JwtHolder)
        {

            AuthenticationResponse JwtHolder1 = await GetRefresh(JwtHolder);
            if (JwtHolder1 != null)
            {
                JwtHolder.State = JwtHolder1.State;
                JwtHolder.JWT = JwtHolder1.JWT;
                JwtHolder.RT = JwtHolder1.RT;
            }
            

            string payload = JsonConvert.SerializeObject(new
            {
                newTitle = todotask.Title,
                newDescription = todotask.Description,  
                newIsDone = todotask.IsDone,        
            });
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("jwt", JwtHolder.JWT.Value.Value);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"http://localhost:44302/api/changeTask/{selectedtask.Id}", content);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            ToDoTaskEditResponse StatusHolder = JsonConvert.DeserializeObject<ToDoTaskEditResponse>(responseContent);
            return StatusHolder.state;  
        }
        public static async Task<TaskGroup> AddTaskGroupAsync(TaskGroup taskGroup, AuthenticationResponse JwtHolder)
        {

            AuthenticationResponse JwtHolder1 = await GetRefresh(JwtHolder);
            if (JwtHolder1 != null)
            {
                JwtHolder.State = JwtHolder1.State;
                JwtHolder.JWT = JwtHolder1.JWT;
                JwtHolder.RT = JwtHolder1.RT;
            }
            var taskids = new List<long>();
            foreach(var task in taskGroup.Tasks)
            {
                ;
                taskids.Add(task.Id);
            } 
            
            var payload = JsonConvert.SerializeObject(new
            {
                groupTitle = taskGroup.Title,
                tasksId = taskids
            });
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("jwt", JwtHolder.JWT.Value.Value);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(@"http://localhost:44302/api/createGroupAndAddTasks", content);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            TaskGroupAddResponse IdHolder = JsonConvert.DeserializeObject<TaskGroupAddResponse>(responseContent);
            taskGroup.Id = IdHolder.entryid;
            return taskGroup;
        }
        public static async Task<UserDataGettingState> EditTaskGroupAsync(TaskGroup SelectedtaskGroup, TaskGroup taskGroup, AuthenticationResponse JwtHolder)
        {

            AuthenticationResponse JwtHolder1 = await GetRefresh(JwtHolder);
            if (JwtHolder1 != null)
            {
                JwtHolder.State = JwtHolder1.State;
                JwtHolder.JWT = JwtHolder1.JWT;
                JwtHolder.RT = JwtHolder1.RT;
            }
            var taskids = new List<long>();
            foreach (var task in taskGroup.Tasks)
            { 
                taskids.Add(task.Id);
            }
            string payload = JsonConvert.SerializeObject(new
            {
                      
                newGroupTitle = taskGroup.Title,
                newTasksId = taskids,

            });
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("jwt", JwtHolder.JWT.Value.Value);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"http://localhost:44302/api/changeGroup/{SelectedtaskGroup.Id}", content);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            ToDoTaskEditResponse StatusHolder = JsonConvert.DeserializeObject<ToDoTaskEditResponse>(responseContent);
            return StatusHolder.state;
        }
        public static async Task<UserDataGettingState> DeleteTaskGroupAsync(TaskGroup taskGroup, AuthenticationResponse JwtHolder)
        {
            AuthenticationResponse JwtHolder1 = await GetRefresh(JwtHolder);
            if (JwtHolder1 != null)
            {
                JwtHolder.State = JwtHolder1.State;
                JwtHolder.JWT = JwtHolder1.JWT;
                JwtHolder.RT = JwtHolder1.RT;
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("jwt", JwtHolder.JWT.Value.Value);
            var response = await client.DeleteAsync($"http://localhost:44302/api/deleteGroup/{taskGroup.Id}");
            var responseContent = response.Content.ReadAsStringAsync().Result;
            TaskGroupRemoveResponse StatusHolder = JsonConvert.DeserializeObject<TaskGroupRemoveResponse>(responseContent);
            return StatusHolder.state;
        }
        public static async Task<AuthenticationResponse> GetRefresh( AuthenticationResponse JwtHolder)
        {
            if (JwtHolder.JWT.Value.ExpiresAt.AddSeconds(-1).CompareTo(DateTimeOffset.Now) <= 0)
            {
                string payload = JsonConvert.SerializeObject(new
                {
                    refreshToken = JwtHolder.RT.Value,
                });
                HttpClient client = new HttpClient();
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(@"http://localhost:44302/api/auth/refresh", content);
                var responseContent = response.Content.ReadAsStringAsync().Result;
                AuthenticationResponse JwtHolder1 = JsonConvert.DeserializeObject<AuthenticationResponse>(responseContent);
                return JwtHolder1;
               
            }
            else
            {
                return null;
            }
         
           
        }

    }
}
