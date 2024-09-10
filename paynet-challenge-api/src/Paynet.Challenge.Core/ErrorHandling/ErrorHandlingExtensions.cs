using Paynet.Challenge.DataContract.V1;

namespace Paynet.Challenge.Core.ErrorHandling.ErrorMessages
{
	static public class ErrorHandlingExtensions
	{
		static public void AddUserAlreadyExistsError(this OperationResponse self)
		{
			var error = ApplicationErrors.GetErrorKeyValuePair("UserAlreadyExists");
			self.AddError(error.Value, "Um usuário com esse e-mail já existe.");
		}

		static public void AddUserConfirmPasswordMustBeEqualError(this OperationResponse self)
		{
			var error = ApplicationErrors.GetErrorKeyValuePair("UserConfirmPasswordMustBeEqual");
			self.AddError(error.Value, "As senhas devem ser iguais.");
		}

		static public void AddGenericFieldsValidationError(this OperationResponse self, string field, string errorMessage)
		{
			var error = ApplicationErrors.GetErrorKeyValuePair("GenericFieldsValidationError");
			self.AddError(error.Value, errorMessage, field);
		}

		static public void AddCepNotFoundError(this OperationResponse self)
		{
			var error = ApplicationErrors.GetErrorKeyValuePair("CepNotFound");
			self.AddError(error.Value, "CEP não encontrado.");
		}

		static public void AddInvalidUserOrPasswordError(this OperationResponse self)
		{
			var error = ApplicationErrors.GetErrorKeyValuePair("InvalidUserOrPassword");
			self.AddError(error.Value, "Usuário ou senha inválido.");
		}

		static public void AddInvalidForgotPasswordCodeError(this OperationResponse self)
		{
			var error = ApplicationErrors.GetErrorKeyValuePair("InvalidForgotPasswordCode");
			self.AddError(error.Value, "Código de recuperação de senha inválido.");
		}
	}
}
