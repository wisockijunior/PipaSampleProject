using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookmark 
{
    // step3
    // Environment selection & build script


    /*
        3.1. Unity project configuration

        Create an empty Unity project. Assuming the game would have online features and we’ll
        have 3 environments (e.g. test, homologation and production, with different server URL’s),
        create a way to define which environment to connect. When hitting ‘Play’, the game must display
        which environment is selected.
     */

    /// <summary>
    /// 3.1. Unity project configuration    
    /// </summary>
    public enum step_3_1 { };

    /*
        3.2. Build script

        Create a batch or shell script that receives the platform and the environment as
        arguments, and outputs the final build. Android and Windows support is enough for the scope of
        this test. You can offer any other arguments you want to increase the script’s flexibility.
   */

    /// <summary>
    /// 3.2. Build script
    /// </summary>
    public enum step_3_2 { };

}
