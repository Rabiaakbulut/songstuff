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
        public MusicModel GetProjectById(int id)
        {
            var projects = GetProjects();
            MusicModel query = projects.FirstOrDefault(x => x.id == id);
            return query;
        }

    }
}
