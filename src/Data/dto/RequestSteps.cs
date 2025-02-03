namespace Cuby.Data.dto
{
    public enum RequestSteps
    {
        RecievedRequest,
        WaitingForUserCapture,
        RecievedUserCapture,
        WaitingForGetCubeString,
        RecievedCubeString,
        WaitingForUserCorrection,
        RecievedUserCorrection,
        WaitingForSolve,
        RecievedSolution,
        WaitingForReturnToClient,
        ReturnedToClient
    }
}
