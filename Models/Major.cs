﻿using System.ComponentModel;

namespace StudentManager.Models
{
    public class Major : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _id;
        private string? _name;
        private string? _description;
		public string _responsable;
        private bool _isSelected;


        public int MajorId
		{
			get => _id;
			set
			{
				if (_id != value)
				{
					_id = value;
					OnPropertyChanged(nameof(MajorId));
				}
			}
		}
        public required string? Name
		{
			get => _name;
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged(nameof(Name));
				}
			}
		}
        public required string? Description
		{
			get => _description;
			set
			{
				if (_description != value)
				{
					_description = value;
					OnPropertyChanged(nameof(Description));
				}
			}
		}
        public required string? Responsable
        {
            get => _responsable;
            set
            {
                if (_responsable != value)
                {
                    _responsable = value;
                    OnPropertyChanged(nameof(Responsable));
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
    }
}
