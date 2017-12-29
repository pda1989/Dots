using System;
using System.Windows.Input;
using Dots.Core.Field;
using Dots.Core.Game;
using Dots.UI.Models;
using FreshMvvm;
using Xamarin.Forms;

namespace Dots.UI.ViewModels
{
    public class GamePageModel : FreshBasePageModel
    {
        #region Fields

        private Field _field;
        private Game _game;
        private string _player;
        private Color _playerColor;
        private string _score;

        #endregion

        #region Properties

        public Field Field
        {
            get => _field;
            set
            {
                _field = value;
                RaisePropertyChanged();
            }
        }

        public string Player
        {
            get => _player;
            set
            {
                _player = value;
                RaisePropertyChanged();
            }
        }

        public string Score
        {
            get => _score;
            set
            {
                _score = value;
                RaisePropertyChanged();
            }
        }

        public ICommand TappedCommand { get; set; }

        public Color PlayerColor
        {
            get => _playerColor;
            set
            {
                _playerColor = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        public override void Init(object initData)
        {
            base.Init(initData);

            _game = new Game();
            _game.OnFieldChanged += newField =>
                Field = newField;
            _game.Initialyze(10);

            Player = _game.FirstPlayerMove ? "First player" : "Second player";
            PlayerColor = _game.FirstPlayerMove ? Color.Blue : Color.Brown;
            Score = $"Score: {_game.Result.FirstPlayerScore} : {_game.Result.SecondPlayerScore}";

            TappedCommand = new Command(dotModel =>
            {
                if (dotModel is DotModel model)
                    try
                    {
                        _game?.MakeMove(model.Row, model.Column);

                        if (_game != null)
                        {
                            Player = _game.FirstPlayerMove ? "First player" : "Second player";
                            PlayerColor = _game.FirstPlayerMove ? Color.Blue : Color.Brown;
                            Score = $"Score: {_game.Result.FirstPlayerScore} : {_game.Result.SecondPlayerScore}";
                        }
                    }
                    catch (Exception exception)
                    {
                        CurrentPage.DisplayAlert("Error", exception.Message, "OK");
                    }
            });
        }

        #endregion
    }
}