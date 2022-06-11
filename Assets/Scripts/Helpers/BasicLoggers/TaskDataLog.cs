using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDataLog : BaseDataLog
{
    private enum State
    {
        None,
        TaskStart,
        TaskEnd
    }

    private enum Result
    {
        None,
        Success,
        Fail
    }

    public void Start()
    {
        string headers = this.GetMandatoryHeaders();
        headers += ",State";
        headers += ",Duration"; // none for start
        headers += ",Result";
        headers += ",Task Recipe";
        headers += ",Submitted Recipe";
        WriteHeader(this.filename, headers, this.filename);
    }

    public void LogStartEvent(Ingredient[] taskIngredients)
    {
        State state = State.TaskStart;

        string line = "";
        line += state.ToString();
        line += ",none";
        line += ",none";
        string contents = "none";
        if (taskIngredients.Length > 0)
        {
            contents = "";
            foreach (Ingredient i in taskIngredients)
            {
                contents += i.ToTaskString() + "|";
            }
        }

        line += "," + contents;
        line += ",none";
        Write(this.filename, line, this.filename);
    }

    public void LogEndEvent(Ingredient[] task, Ingredient[] submission, bool match, float duration)
    {
        State state = State.TaskEnd;
        Result result = match ? Result.Success : Result.Fail;
        if(submission == null)
        {
            return;
        }
        string line = "";
        line += state.ToString();
        line += "," + duration.ToString();
        line += "," + result.ToString();
        string contents = "none";
        if (task.Length > 0)
        {
            contents = "";
            foreach (Ingredient i in task)
            {
                contents += i.ToTaskString() + "|";
            }
        }
        string potion = "none";
        if (submission.Length > 0)
        {
            potion = "";
            foreach (Ingredient i in submission)
            {
                potion += i.ToString() + "|";
            }
        }
        line += "," + contents;
        line += "," + potion;
        Write(this.filename, line, this.filename);
    }
}
