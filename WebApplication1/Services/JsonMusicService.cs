using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication1.Model;

namespace WebApplication1.Services
{
    public class JsonMusicService
    {
        public JsonMusicService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "json.json"); }
            //direkt yol vermektense data klasorundeki json dosyasını al diyoruz
            //böylece proje başka bilgisayarlarda açılınca sıkıntı olmasın diye
        }
        public IEnumerable<MusicModel> GetProjects()
        {
            using var json = File.OpenText(JsonFileName);
            return JsonSerializer.Deserialize<MusicModel[]>(json.ReadToEnd());
        }
        public void AddProject(MusicModel newproject)
        {
            var projects = GetProjects();
            newproject.id = projects.Max(x => x.id) + 1;
            var temp = projects.ToList();
            temp.Add(newproject);
            IEnumerable<MusicModel> updateprojects = temp.ToArray();
            using var json = File.OpenWrite(JsonFileName);
            JsonSerializer.Serialize<IEnumerable<MusicModel>>(
                new Utf8JsonWriter(json, new JsonWriterOptions { Indented = true }), updateprojects);
        }
        public void UpdateProject(MusicModel newproject)
        {
            var projects = GetProjects();
            MusicModel query = projects.Single(x => x.id == newproject.id);
            if (query != null)
            {
                var temp = projects.ToList();
                temp[temp.IndexOf(query)] = newproject;
                IEnumerable<MusicModel> updateprojects = temp.ToArray();
                JsonWrite(updateprojects);
            }

        }
        public void DeleteProject(int id)
        {
            var projects = GetProjects();
            MusicModel query = projects.Single(x => x.id == id);
            var temp = projects.ToList();
            temp.Remove(query);
            IEnumerable<MusicModel> updateprojects = temp.ToArray();
            JsonWrite(updateprojects);
        }
        public void JsonWrite(IEnumerable<MusicModel> project)
        {
            using var json = File.Create(JsonFileName);
            JsonSerializer.Serialize<IEnumerable<MusicModel>>(
                new Utf8JsonWriter(json, new JsonWriterOptions { Indented = true }), project);
        }
        public MusicModel GetProjectById(int id)
        {
            var projects = GetProjects();
            MusicModel query = projects.FirstOrDefault(x => x.id == id);
            return query;
        }

    }
}
