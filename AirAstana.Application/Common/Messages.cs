namespace AirAstana.Application.Common;

public static class Messages
{
    // Пользователи
    public const string UsernameRequired = "Имя пользователя обязательно";
    public const string UsernameMinLength = "Имя пользователя должно быть не меньше 3 символов";
    public const string PasswordRequired = "Пароль обязателен";
    public const string PasswordMinLength = "Пароль должен быть не меньше 6 символов";
    public const string UserAlreadyExists = "Пользователь уже существует";
    public const string UserRegistered = "Пользователь успешно зарегистрирован (роль ID: {0})";
    public const string UsernameOrPasswordRequired = "Имя пользователя и пароль обязательны для входа";
    public const string UserNotFound = "Пользователь не найден";
    public const string InvalidCredentials = "Неверное имя пользователя или пароль";
    public const string LoginSuccess = "Успешный вход";


    // Рейсы
    public const string FlightAdded = "Рейс успешно добавлен";
    public const string FlightTimeInvalid = "Время вылета должно быть раньше времени прилета";
    public const string FlightAddError = "Не удалось добавить рейс";
    public const string FlightAddException = "Ошибка при добавлении рейса";
    public const string FlightsFound = "Список рейсов получен";
    public const string FlightsNotFound = "Рейсы не найдены";
    public const string FlightNotFound = "Рейс не найден";
    public const string FlightStatusUpdated = "Статус рейса успешно обновлён";
    
    public const string OriginRequired = "Город отправления обязателен";
    public const string DestinationRequired = "Город назначения обязателен";
    public const string InvalidFlightStatus = "Статус рейса некорректный";


    // Общие ошибки
    public const string UnknownError = "Произошла неизвестная ошибка";
    public const string ValidationError = "Ошибка валидации";

}