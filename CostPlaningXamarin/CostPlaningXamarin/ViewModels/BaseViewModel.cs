using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CostPlaningXamarin.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
	{
		Dictionary<string, List<ValidationRule>> _rules = new Dictionary<string, List<ValidationRule>>();

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
		private bool _isValidated = false;
		public bool HasErrors
		{
			get
			{
				if (!_isValidated)
				{
					AllPropertiesValidation();
					_isValidated = true;
				}
				if (!_validationErrors.Any())
				{
					return false;
				}
				return _validationErrors.Any(x => x.Value.Count > 0);
			}
		}
		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
		#endregion
		public IEnumerable GetErrors([CallerMemberName] string propertyName = "")
		{
			if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
				return null;

			return _validationErrors[propertyName];
		}
		protected virtual void InvalidateCommands()
		{

		}
		protected void AddValidationRule(string propertyName, Func<bool> requirement, string errorMesasge)
		{
			if (!_rules.ContainsKey(propertyName))
			{
				_rules.Add(propertyName, new List<ValidationRule>());
			}
			_rules[propertyName].Add(new ValidationRule(propertyName, requirement, errorMesasge));
		}

		protected virtual void PropertyValidation([CallerMemberName] string propertyName = "")
		{
			List<ValidationRule> propertyRules;
			if (_rules.TryGetValue(propertyName, out propertyRules))
			{
				foreach (var rule in propertyRules)
				{
					CheckValidationRule(rule);
				}
			}
			InvalidateCommands();
			OnPropertyChanged(nameof(HasErrors));
		}

		protected virtual void AllPropertiesValidation()
		{
			foreach (var propertyRules in _rules)
			{
				foreach (var rule in propertyRules.Value)
				{
					CheckValidationRule(rule);
				}
			}
			InvalidateCommands();
			OnPropertyChanged(nameof(HasErrors));
		}

		protected readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

		protected void CheckValidationRule(string propertyName, Func<bool> condition, string message)
		{
			if (!condition())
			{
				AddValidationError(propertyName, message);
			}
			else
			{
				RemoveValidationError(propertyName, message);
			}
		}

		protected void CheckValidationRule(ValidationRule rule)
		{
			if (!rule.CheckRequirement())
			{
				AddValidationError(rule.PropertyName, rule.ErrorMessage);
			}
			else
			{
				RemoveValidationError(rule.PropertyName, rule.ErrorMessage);
			}
		}

		protected void AddValidationError(string propertyName, string error)
		{
			if (!_validationErrors.ContainsKey(propertyName))
				_validationErrors[propertyName] = new List<string>();

			if (!_validationErrors[propertyName].Contains(error))
			{
				_validationErrors[propertyName].Insert(0, error);
				RaiseErrorsChanged(propertyName);
			}
		}

		protected void RemoveValidationError(string propertyName, string error)
		{
			if (_validationErrors.ContainsKey(propertyName) &&
				_validationErrors[propertyName].Contains(error))
			{
				_validationErrors[propertyName].Remove(error);
				RaiseErrorsChanged(propertyName);
			}
		}

		protected void RaiseErrorsChanged([CallerMemberName] string propertyName = "")
		{
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

	}

	public class ValidationRule
	{
		private Func<bool> _requirement;

		public ValidationRule(string propertyName, Func<bool> requirement, string errorMessage)
		{
			PropertyName = propertyName;
			_requirement = requirement;
			ErrorMessage = errorMessage;
		}

		public string PropertyName { get; private set; }
		public string ErrorMessage { get; private set; }
		public bool CheckRequirement()
		{
			return _requirement.Invoke();
		}
	}
}
