#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class VisualElementWrapper<T> : IVisualElementWrapper<T> where T : notnull, VisualElement {

        protected T VisualElement { get; }
        T IVisualElementWrapper<T>.VisualElement => VisualElement;

        public VisualElementWrapper(T visualElement) {
            VisualElement = visualElement;
        }

    }
    // Element
    public class ElementWrapper : VisualElementWrapper<VisualElement> {

        public ElementWrapper(VisualElement visualElement) : base( visualElement ) {
        }

    }
    // Label
    public class LabelWrapper : VisualElementWrapper<Label> {

        public string? Text {
            get => VisualElement.text;
            set => VisualElement.text = value;
        }

        public LabelWrapper(Label visualElement) : base( visualElement ) {
        }

    }
    // Button
    public class ButtonWrapper : VisualElementWrapper<Button> {

        public string? Text {
            get => VisualElement.text;
            set => VisualElement.text = value;
        }

        public ButtonWrapper(Button visualElement) : base( visualElement ) {
        }

    }
    public class RepeatButtonWrapper : VisualElementWrapper<RepeatButton> {

        public string? Text {
            get => VisualElement.text;
            set => VisualElement.text = value;
        }

        public RepeatButtonWrapper(RepeatButton visualElement) : base( visualElement ) {
        }

    }
    // Image
    public class ImageWrapper : VisualElementWrapper<Image> {

        public Texture Image {
            get => VisualElement.image;
            set => VisualElement.image = value;
        }
        public Sprite Sprite {
            get => VisualElement.sprite;
            set => VisualElement.sprite = value;
        }
        public VectorImage VectorImage {
            get => VisualElement.vectorImage;
            set => VisualElement.vectorImage = value;
        }
        public Color Color {
            get => VisualElement.tintColor;
            set => VisualElement.tintColor = value;
        }
        public ScaleMode ScaleMode {
            get => VisualElement.scaleMode;
            set => VisualElement.scaleMode = value;
        }
        public Rect Rect {
            get => VisualElement.sourceRect;
            set => VisualElement.sourceRect = value;
        }
        public Rect RectUV {
            get => VisualElement.uv;
            set => VisualElement.uv = value;
        }

        public ImageWrapper(Image visualElement) : base( visualElement ) {
        }

    }
    // TextField
    public class TextFieldWrapper<T> : VisualElementWrapper<BaseField<string?>> where T : notnull {

        public string? Label {
            get => VisualElement.label;
            set => VisualElement.label = value;
        }
        public string? Value {
            get => VisualElement.value;
            set => VisualElement.value = value;
        }

        public TextFieldWrapper(BaseField<string?> visualElement) : base( visualElement ) {
            Assert.Operation.Message( $"TextWrapper {this} is invalid" ).Valid( this is TextFieldWrapper<string> );
        }

    }
    // PopupField
    public class PopupFieldWrapper<T> : VisualElementWrapper<PopupField<T?>> where T : notnull {

        public string? Label {
            get => VisualElement.label;
            set => VisualElement.label = value;
        }
        public T? Value {
            get => VisualElement.value;
            set => VisualElement.value = value;
        }
        public T?[] Choices {
            get => VisualElement.choices.ToArray();
            set => VisualElement.choices = value.ToList();
        }
        public (T? Value, T?[] Choices) ValueChoices {
            get => (VisualElement.value, VisualElement.choices.ToArray());
            set => (VisualElement.value, VisualElement.choices) = (value.Value, value.Choices.ToList());
        }

        public PopupFieldWrapper(PopupField<T?> visualElement) : base( visualElement ) {
        }

    }
    // SliderField
    public class SliderFieldWrapper<T> : VisualElementWrapper<BaseSlider<T>> where T : struct, IComparable<T> {

        public string? Label {
            get => VisualElement.label;
            set => VisualElement.label = value;
        }
        public T Value {
            get => VisualElement.value;
            set => VisualElement.value = value;
        }
        public T Min {
            get => VisualElement.lowValue;
            set => VisualElement.lowValue = value;
        }
        public T Max {
            get => VisualElement.highValue;
            set => VisualElement.highValue = value;
        }
        public (T Value, T Min, T Max) ValueMinMax {
            get => (VisualElement.value, VisualElement.lowValue, VisualElement.highValue);
            set => (VisualElement.value, VisualElement.lowValue, VisualElement.highValue) = (value.Value, value.Min, value.Max);
        }

        public SliderFieldWrapper(BaseSlider<T> visualElement) : base( visualElement ) {
        }

    }
    // ToggleField
    public class ToggleFieldWrapper<T> : VisualElementWrapper<Toggle> where T : struct, IComparable<T> {

        public string? Label {
            get => VisualElement.label;
            set => VisualElement.label = value;
        }
        public bool Value {
            get => VisualElement.value;
            set => VisualElement.value = value;
        }

        public ToggleFieldWrapper(Toggle visualElement) : base( visualElement ) {
            Assert.Operation.Message( $"ToggleWrapper {this} is invalid" ).Valid( this is ToggleFieldWrapper<bool> );
        }

    }
    // ViewSlot
    public class ViewSlotWrapper<TView> : VisualElementWrapper<VisualElement> where TView : notnull, UIViewBase {

        public TView? View { get; private set; }

        public ViewSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Set(TView view) {
            Clear();
            View = view;
            VisualElement.Add( view.VisualElement );
        }
        public void Clear() {
            if (View != null) {
                VisualElement.Remove( View.VisualElement );
                View = null;
            }
        }

    }
    // ViewList
    public class ViewListWrapper<TView> : VisualElementWrapper<VisualElement> where TView : notnull, UIViewBase {

        private List<TView> Views_ { get; } = new List<TView>();
        public IReadOnlyList<TView> Views => Views_;

        public ViewListWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Add(TView view) {
            Views_.Add( view );
            VisualElement.Add( view.VisualElement );
        }
        public void Remove(TView view) {
            VisualElement.Remove( view.VisualElement );
            Views_.Remove( view );
        }
        public bool Contains(TView view) {
            return Views_.Contains( view );
        }
        public void Clear() {
            foreach (var view in Views_) {
                VisualElement.Remove( view.VisualElement );
            }
            Views_.Clear();
        }

    }
    // ViewStack
    public class ViewStackWrapper<TView> : VisualElementWrapper<VisualElement> where TView : notnull, UIViewBase {

        private Stack<TView> Views_ { get; } = new Stack<TView>();
        public IReadOnlyCollection<TView> Views => Views_;

        public ViewStackWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Push(TView view) {
            if (Views_.TryPeek( out var top )) {
                top.SetEnabled( false );
                top.SetDisplayed( false );
            }
            Views_.Push( view );
            VisualElement.Add( view.VisualElement );
        }
        public void Pop() {
            VisualElement.Remove( Views_.Peek().VisualElement );
            Views_.Pop();
            if (Views_.TryPeek( out var top )) {
                top.SetEnabled( true );
                top.SetDisplayed( true );
            }
        }
        public bool Contains(TView view) {
            return Views_.Contains( view );
        }
        public void Clear() {
            while (Views_.TryPop( out var view )) {
                VisualElement.Remove( view.VisualElement );
            }
        }

    }
}
