using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using InfoneticaTask.Models;

namespace InfoneticaTask.Services
{
    public class WorkflowService
    {
        private readonly string _definitionsFile = "workflows.json";
        private readonly string _instancesFile = "instances.json";

        private readonly ConcurrentDictionary<string, WorkflowDefinition> _definitions;
        private readonly ConcurrentDictionary<string, WorkflowInstance> _instances;

        public WorkflowService()
        {
            _definitions = LoadData<ConcurrentDictionary<string, WorkflowDefinition>>(_definitionsFile) ?? new();
            _instances = LoadData<ConcurrentDictionary<string, WorkflowInstance>>(_instancesFile) ?? new();
        }

        // Create Workflow Definition
        public (bool IsSuccess, string Message) CreateWorkflow(WorkflowDefinition def)
        {
            if (_definitions.ContainsKey(def.Id)) return (false, "Workflow already exists");
            if (def.States.Count == 0) return (false, "No states provided");
            if (def.States.Count(s => s.IsInitial) != 1) return (false, "Must have exactly one initial state");
            if (def.States.Select(s => s.Id).Distinct().Count() != def.States.Count) return (false, "Duplicate state IDs");

            _definitions[def.Id] = def;
            SaveData(_definitionsFile, _definitions);
            return (true, $"Workflow {def.Id} created");
        }

        // Get Workflow by ID
        public WorkflowDefinition? GetWorkflow(string id)
        {
            _definitions.TryGetValue(id, out var def);
            return def;
        }

        // Get All Workflows
        public List<WorkflowDefinition> GetAllWorkflows() => _definitions.Values.ToList();

        // Start Workflow Instance
        public WorkflowInstance? StartWorkflowInstance(string workflowId)
        {
            if (!_definitions.TryGetValue(workflowId, out var def)) return null;

            var initialState = def.States.FirstOrDefault(s => s.IsInitial && s.Enabled);
            if (initialState == null) return null;

            var instance = new WorkflowInstance
            {
                WorkflowId = workflowId,
                CurrentState = initialState.Id
            };

            _instances[instance.Id] = instance;
            SaveData(_instancesFile, _instances);
            return instance;
        }

        // Execute Action
        public (bool IsSuccess, string Message) ExecuteAction(string instanceId, string actionId)
        {
            if (!_instances.TryGetValue(instanceId, out var instance)) return (false, "Instance not found");
            if (!_definitions.TryGetValue(instance.WorkflowId, out var def)) return (false, "Workflow not found");

            var action = def.Actions.FirstOrDefault(a => a.Id == actionId);
            if (action == null || !action.Enabled) return (false, "Action invalid or disabled");
            if (!action.FromStates.Contains(instance.CurrentState)) return (false, "Action not allowed from current state");

            var toState = def.States.FirstOrDefault(s => s.Id == action.ToState && s.Enabled);
            if (toState == null) return (false, "Target state invalid or disabled");
            if (def.States.First(s => s.Id == instance.CurrentState).IsFinal) return (false, "Current state is final");

            instance.CurrentState = toState.Id;
            instance.History.Add(new HistoryEntry(actionId, DateTime.UtcNow));

            SaveData(_instancesFile, _instances);
            return (true, $"Action executed, new state: {toState.Name}");
        }

        // Get Instance by ID
        public WorkflowInstance? GetInstance(string id)
        {
            _instances.TryGetValue(id, out var instance);
            return instance;
        }

        public List<WorkflowInstance> GetAllInstances() => _instances.Values.ToList();

        // Generic JSON Persistence
        private T? LoadData<T>(string fileName)
        {
            if (!File.Exists(fileName)) return default;
            var json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<T>(json);
        }

        private void SaveData<T>(string fileName, T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }
    }
}
