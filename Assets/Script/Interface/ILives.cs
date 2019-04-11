namespace ru.lifanoff.Intarface {

    /// <summary>
    /// Интерфейс для всего, что имеет "жизни".
    /// </summary>
    public interface ILives {
        void IncreaseLive(int count = 1);
        void DecreaseLive(int count = 1);
    }

}