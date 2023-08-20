using UnityEngine;

public class GameAdministrator
{
    public static void StartGame()
    {
        DayManager.instance.StartFirstDay();
    }

    public static void GoTrue()
    {
        SceneController.TransitToSceneFade("Ending",3.0f,Interpolation.Easing.Style.Quart,Color.white,null,Transition.IOType.OutIn);
    }

    public static void GoBad()
    {
        SceneController.TransitToSceneFade("Ending",3.0f,Interpolation.Easing.Style.Quart,Color.black,null,Transition.IOType.OutIn);
    }

    public static void ShutDown()
    {
        // shut down application
        Application.Quit();
    }
}
