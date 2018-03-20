// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Microsoft.Toolkit.Uwp.UI.Extensions;

namespace ContosoInsurance.Controls
{
    [TemplatePart(Name = RootName, Type = typeof(Grid))]
    [TemplatePart(Name = OldTextblockName, Type = typeof(TextBlock))]
    [TemplatePart(Name = NewTextblockName, Type = typeof(TextBlock))]
    public sealed class AnimatedTextBlock : Control
    {
        #region Fileds

        private const string RootName = "Root";
        private const string OldTextblockName = "OldTextBlock";
        private const string NewTextblockName = "NewTextBlock";
        private const float ExtraOffset = 4.0f;

        private Compositor _compositor;

        private Grid _root;
        private Visual _rootVisual;

        private TextBlock _oldTextBlock;
        private Visual _oldTextBlockVisual;
        private TextBlock _newTextBlock;
        private Visual _newTextBlockVisual;

        private bool _notInitialized;

        #endregion

        public AnimatedTextBlock()
        {
            DefaultStyleKey = typeof(AnimatedTextBlock);

            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        #region Properties

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AnimatedTextBlock),
                new PropertyMetadata(string.Empty, (d, e) =>
                {
                    var self = (AnimatedTextBlock)d;

                    if (self._newTextBlockVisual != null)
                    {
                        self.AnimateTextBlocks((string)e.NewValue, (string)e.OldValue);
                    }
                }));

        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(AnimatedTextBlock), new PropertyMetadata(TextAlignment.Right));

        public TextLineBounds TextLineBounds
        {
            get => (TextLineBounds)GetValue(TextLineBoundsProperty);
            set => SetValue(TextLineBoundsProperty, value);
        }

        public static readonly DependencyProperty TextLineBoundsProperty =
            DependencyProperty.Register("TextLineBounds", typeof(TextLineBounds), typeof(AnimatedTextBlock), new PropertyMetadata(TextLineBounds.Full));

        public TransitionDirection TransitionDirection
        {
            get => (TransitionDirection)GetValue(TransitionDirectionProperty);
            set => SetValue(TransitionDirectionProperty, value);
        }

        public static readonly DependencyProperty TransitionDirectionProperty =
            DependencyProperty.Register("TransitionDirection", typeof(TransitionDirection), typeof(AnimatedTextBlock), new PropertyMetadata(TransitionDirection.BottomToTop));

        public static CubicBezierEasingFunction EaseInOutCubic(Compositor compositor) =>
            compositor.CreateCubicBezierEasingFunction(new Vector2(0.645f, 0.045f), new Vector2(0.355f, 1.0f));

        #endregion

        #region Overrides

        /// <exception cref="NullReferenceException">Condition.</exception>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _root = GetTemplateChild<Grid>(RootName);
            _rootVisual = VisualExtensions.GetVisual(_root); 

            _compositor = _rootVisual.Compositor;

            _oldTextBlock = GetTemplateChild<TextBlock>(OldTextblockName);
            _oldTextBlockVisual = VisualExtensions.GetVisual(_oldTextBlock);

            _newTextBlock = GetTemplateChild<TextBlock>(NewTextblockName);
            _newTextBlockVisual = VisualExtensions.GetVisual(_newTextBlock);

            if (_notInitialized)
            {
                Initialize();
            }
        }

        #endregion

        #region Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            if (_root == null)
            {
                _notInitialized = true;
            }
            else
            {
                Initialize();
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateRootSize();
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            ClipRoot();

            if (!string.IsNullOrWhiteSpace(Text))
            {
                AnimateTextBlocks(Text);
            }
        }

        private void ClipRoot()
        {
            var clip = _compositor.CreateInsetClip();
            clip.TopInset = 0.0f;
            clip.BottomInset = 0.0f;
            clip.LeftInset = 0.0f;
            clip.RightInset = 0.0f;

            _rootVisual.Clip = clip;
        }

        private void UpdateRootSize() =>
            _rootVisual.Size = new Vector2((float)_root.ActualWidth, (float)_root.ActualHeight);

        private void AnimateTextBlocks(string newValue, string oldValue = "")
        {
            _oldTextBlock.Text = oldValue;
            _newTextBlock.Text = newValue;

            AnimationAxis axis;
            int offsetMultipler;
            float offset;

            switch (TransitionDirection)
            {
                case TransitionDirection.TopToBottom:
                    axis = AnimationAxis.Y;
                    offsetMultipler = 1;
                    offset = _rootVisual.Size.Y + ExtraOffset;
                    break;

                default:
                    //case TransitionDirection.BottomToTop:
                    axis = AnimationAxis.Y;
                    offsetMultipler = -1;
                    offset = _rootVisual.Size.Y + ExtraOffset;
                    break;

                case TransitionDirection.LeftToRight:
                    axis = AnimationAxis.X;
                    offsetMultipler = 1;
                    offset = _rootVisual.Size.X + ExtraOffset;
                    break;

                case TransitionDirection.RightToLeft:
                    axis = AnimationAxis.X;
                    offsetMultipler = -1;
                    offset = _rootVisual.Size.X + ExtraOffset;
                    break;
            }

            var easing = EaseInOutCubic(_compositor);

            StartOffsetAnimation(_newTextBlockVisual, axis, -offsetMultipler * offset, 0, 500, easing: easing);
            StartOffsetAnimation(_oldTextBlockVisual, axis, 0, offsetMultipler * offset, 500, easing: easing);
        }

        #endregion

        #region Helpers

        /// <exception cref="NullReferenceException">Condition.</exception>
        public T GetTemplateChild<T>(string name, string message = null) where T : DependencyObject
        {
            if (!(GetTemplateChild(name) is T child))
            {
                if (message == null)
                {
                    message = $"{name} should not be null! Check the default Generic.xaml.";
                }

                throw new NullReferenceException(message);
            }

            return child;
        }

        private static void StartOffsetAnimation(Visual visual, AnimationAxis axis, float? from = null, float to = 0,
            double duration = 800, int delay = 0, CompositionEasingFunction easing = null)
        {
            var compositor = visual.Compositor;
            var animation = compositor.CreateScalarKeyFrameAnimation();

            animation.Duration = TimeSpan.FromMilliseconds(duration);
            if (!delay.Equals(0)) animation.DelayTime = TimeSpan.FromMilliseconds(delay);
            if (from.HasValue) animation.InsertKeyFrame(0.0f, from.Value, easing);
            animation.InsertKeyFrame(1.0f, to, easing);

            visual.StartAnimation($"Offset.{axis}", animation);
        }

        #endregion
    }
}
