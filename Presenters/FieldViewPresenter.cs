public class FieldViewPresenter
{
    private readonly FieldView fieldView;

    private readonly FieldLogic fieldLogic;
    private readonly ServerTransmitter serverTransmitter;
    private readonly ServerCalls serverCalls;

    public FieldViewPresenter(FieldView fieldView, FieldLogic fieldLogic, ServerContainer serverContainer)
    {
        this.fieldView = fieldView;
        this.fieldLogic = fieldLogic;
        this.serverTransmitter = serverContainer.ServerTransmitter;
        this.serverCalls = serverContainer.ServerCalls;
    }

    public void Enable()
    {
        fieldLogic.OnFieldChanged += UpdateFieldView;
        fieldLogic.OnGameOver += GameOver;

        serverTransmitter.OnPlayerLeftRoom += PrepareRoom;
        serverCalls.OnGameStarted += StartGame;
        serverCalls.OnServerFieldUpdated += UpdateFieldState;
    }

    public void Disable()
    {
        fieldLogic.OnFieldChanged -= UpdateFieldView;
        fieldLogic.OnGameOver -= GameOver;

        serverTransmitter.OnPlayerLeftRoom -= PrepareRoom;
        serverCalls.OnGameStarted -= StartGame;
        serverCalls.OnServerFieldUpdated -= UpdateFieldState;
    }

    private void UpdateFieldView(int numberOfActivated, char sign, string message)
    {
        if (sign == '0')
            fieldView.PutOnFieldZero(numberOfActivated);
        else
            fieldView.PutOnFieldCross(numberOfActivated);

        fieldView.ShowMessage(message);
    }

    private void GameOver(string message)
    {
        fieldView.DisablePlayingField();
        fieldView.ShowMessage(message);
    }

    private void PrepareRoom()
    {
        fieldView.ClearField();
        fieldView.ShowMessage("������� ������� ������");
    }

    private void StartGame(bool isThisClientHost)
    {
        fieldLogic.RestartGame(isThisClientHost);
        fieldView.ClearField();
        fieldView.ShowMessage(fieldLogic.GetNextStepMessage());
    }

    private void UpdateFieldState(int numberOfActivated)
    {
        fieldLogic.UpdateFieldState(numberOfActivated);
    }
}
