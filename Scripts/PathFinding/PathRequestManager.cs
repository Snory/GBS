using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System;
using UnityEngine;

public delegate void PathResult(IPathFindable[] path, bool result);

public class PathRequestManager : Singleton<PathRequestManager>
{

    private Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
    private PathRequest _currentPathRequest;

    private bool _isProcessing;



    // Start is called before the first frame update
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, PathResult pathResultCallback)
    {
        PathRequest newPathRequest = new PathRequest(pathStart, pathEnd, pathResultCallback);
        Instance._pathRequestQueue.Enqueue(newPathRequest);
        Instance.TryProcesNext();

    }

    private void TryProcesNext()
    {
        //TODO: asynchronnous?
        if(!_isProcessing && _pathRequestQueue.Count > 0)
        {
            _currentPathRequest = _pathRequestQueue.Dequeue();
            _isProcessing = true;
            PathFinding.Instance.StartFindPath(_currentPathRequest._pathStart, _currentPathRequest._pathEnd);
        }

    }

    public void PathProcessingFinished(IPathFindable[] path, bool success)
    {
        _currentPathRequest._pathResultCallback(path, success);
        _isProcessing = false;
        TryProcesNext();
    }

    struct PathRequest
    {
        public Vector3 _pathStart;
        public Vector3 _pathEnd;
        public PathResult _pathResultCallback;

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, PathResult pathResultCallback)
        {
            _pathStart = pathStart;
            _pathEnd = pathEnd;
            _pathResultCallback = pathResultCallback;
        }
    }
    
}
