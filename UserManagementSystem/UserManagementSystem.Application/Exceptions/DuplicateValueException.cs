namespace UserManagementSystem.Application.Exceptions;

public class DuplicateValueException(string message) : Exception(message);