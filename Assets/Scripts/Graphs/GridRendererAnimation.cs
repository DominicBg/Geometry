using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRendererAnimation : GridRenderer {
    
 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            showCols = showRows = false;
            generateMesh = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            showCols = showRows = true;
            generateMesh = false;
        }

        base.Update();
    }
}
