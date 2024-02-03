#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class VisualElementWrapper {

        protected internal VisualElement VisualElement { get; }

        public bool IsEnabled {
            get => VisualElement.enabledSelf;
            set => VisualElement.SetEnabled( value );
        }
        public bool IsDisplayed {
            get => VisualElement.IsDisplayed();
            set => VisualElement.SetDisplayed( value );
        }
        public bool IsValid {
            get => VisualElement.IsValid();
            set => VisualElement.SetValid( value );
        }

        public IReadOnlyList<string> Classes {
            get => (IReadOnlyList<string>) VisualElement.GetClasses();
        }

        public VisualElementWrapper(VisualElement visualElement) {
            VisualElement = visualElement;
        }

        public void AddClass(string @class) {
            VisualElement.AddToClassList( @class );
        }
        public void RemoveClass(string @class) {
            VisualElement.RemoveFromClassList( @class );
        }
        public void ToggleClass(string @class) {
            VisualElement.ToggleInClassList( @class );
        }
        public void EnableClass(string @class, bool isEnabled) {
            VisualElement.EnableInClassList( @class, isEnabled );
        }
        public bool ContainsClass(string @class) {
            return VisualElement.ClassListContains( @class );
        }
        public void ClearClasses() {
            VisualElement.ClearClassList();
        }

    }
    public abstract class VisualElementWrapper<T> : VisualElementWrapper where T : VisualElement {

        protected internal new T VisualElement => (T) base.VisualElement;

        public VisualElementWrapper(T visualElement) : base( visualElement ) {
        }

    }
    public static class VisualElementWrapperExtensions {

        // Wrap
        public static ElementWrapper Wrap(this VisualElement visualElement) {
            return new ElementWrapper( visualElement );
        }
        public static LabelWrapper Wrap(this Label visualElement) {
            return new LabelWrapper( visualElement );
        }
        public static ButtonWrapper Wrap(this Button visualElement) {
            return new ButtonWrapper( visualElement );
        }
        public static RepeatButtonWrapper Wrap(this RepeatButton visualElement) {
            return new RepeatButtonWrapper( visualElement );
        }
        public static ImageWrapper Wrap(this Image visualElement) {
            return new ImageWrapper( visualElement );
        }
        public static TextFieldWrapper<string> Wrap(this BaseField<string?> visualElement) {
            return new TextFieldWrapper<string>( visualElement );
        }
        public static PopupFieldWrapper<T> Wrap<T>(this PopupField<T?> visualElement) where T : notnull {
            return new PopupFieldWrapper<T>( visualElement );
        }
        public static SliderFieldWrapper<T> Wrap<T>(this BaseSlider<T> visualElement) where T : struct, IComparable<T> {
            return new SliderFieldWrapper<T>( visualElement );
        }
        public static ToggleFieldWrapper<bool> Wrap(this Toggle visualElement) {
            return new ToggleFieldWrapper<bool>( visualElement );
        }

        // AsSlot
        public static SlotWrapper AsSlot(this VisualElement visualElement) {
            return new SlotWrapper( visualElement );
        }

        // OnEvent/TrickleDown
        public static void OnEventTrickleDown<TEvt>(this VisualElementWrapper wrapper, EventCallback<TEvt> callback) where TEvt : EventBase<TEvt>, new() {
            wrapper.VisualElement.RegisterCallback( callback, TrickleDown.TrickleDown );
        }
        public static void OnEventTrickleDown<TEvt, TArg>(this VisualElementWrapper wrapper, EventCallback<TEvt, TArg> callback, TArg arg) where TEvt : EventBase<TEvt>, new() {
            wrapper.VisualElement.RegisterCallback( callback, arg, TrickleDown.TrickleDown );
        }

        // OnEvent/BubbleUp
        public static void OnEvent<TEvt>(this VisualElementWrapper wrapper, EventCallback<TEvt> callback) where TEvt : EventBase<TEvt>, new() {
            wrapper.VisualElement.RegisterCallback( callback, TrickleDown.NoTrickleDown );
        }
        public static void OnEvent<TEvt, TArg>(this VisualElementWrapper wrapper, EventCallback<TEvt, TArg> callback, TArg arg) where TEvt : EventBase<TEvt>, new() {
            wrapper.VisualElement.RegisterCallback( callback, arg, TrickleDown.NoTrickleDown );
        }

        // OnAttachToPanel
        public static void OnAttachToPanel(this VisualElementWrapper wrapper, Action? callback) {
            wrapper.VisualElement.RegisterCallback<AttachToPanelEvent>( evt => callback?.Invoke() );
        }
        public static void OnAttachToPanel(this VisualElementWrapper wrapper, Action<AttachToPanelEvent>? callback) {
            wrapper.VisualElement.RegisterCallback<AttachToPanelEvent>( evt => callback?.Invoke( evt ) );
        }

        // OnDetachFromPanel
        public static void OnDetachFromPanel(this VisualElementWrapper wrapper, Action? callback) {
            wrapper.VisualElement.RegisterCallback<DetachFromPanelEvent>( evt => callback?.Invoke() );
        }
        public static void OnDetachFromPanel(this VisualElementWrapper wrapper, Action<DetachFromPanelEvent>? callback) {
            wrapper.VisualElement.RegisterCallback<DetachFromPanelEvent>( evt => callback?.Invoke( evt ) );
        }

        // OnGeometryChanged
        public static void OnGeometryChanged(this VisualElementWrapper wrapper, Action? callback) {
            wrapper.VisualElement.RegisterCallback<GeometryChangedEvent>( evt => callback?.Invoke() );
        }
        public static void OnGeometryChanged(this VisualElementWrapper wrapper, Action<GeometryChangedEvent> callback) {
            wrapper.VisualElement.RegisterCallback<GeometryChangedEvent>( evt => callback?.Invoke( evt ) );
        }

        // OnFocusIn
        public static void OnFocusIn(this VisualElementWrapper wrapper, Action? callback) {
            // Event sent immediately before an element gains focus. This event trickles down and bubbles up.
            wrapper.VisualElement.RegisterCallback<FocusInEvent>( evt => callback?.Invoke() );
        }
        public static void OnFocusIn(this VisualElementWrapper wrapper, Action<FocusInEvent>? callback) {
            // Event sent immediately before an element gains focus. This event trickles down and bubbles up.
            wrapper.VisualElement.RegisterCallback<FocusInEvent>( evt => callback?.Invoke( evt ) );
        }

        // OnFocus
        public static void OnFocus(this VisualElementWrapper wrapper, Action? callback) {
            // Event sent immediately after an element has gained focus. This event trickles down (and does not bubbles up).
            wrapper.VisualElement.RegisterCallback<FocusEvent>( evt => callback?.Invoke() );
        }
        public static void OnFocus(this VisualElementWrapper wrapper, Action<FocusEvent>? callback) {
            // Event sent immediately after an element has gained focus. This event trickles down (and does not bubbles up).
            wrapper.VisualElement.RegisterCallback<FocusEvent>( evt => callback?.Invoke( evt ) );
        }

        // OnFocusOut
        public static void OnFocusOut(this VisualElementWrapper wrapper, Action? callback) {
            // Event sent immediately before an element loses focus. This event trickles down and bubbles up.
            wrapper.VisualElement.RegisterCallback<FocusInEvent>( evt => callback?.Invoke() );
        }
        public static void OnFocusOut(this VisualElementWrapper wrapper, Action<FocusOutEvent>? callback) {
            // Event sent immediately before an element loses focus. This event trickles down and bubbles up.
            wrapper.VisualElement.RegisterCallback<FocusOutEvent>( evt => callback?.Invoke( evt ) );
        }

        // OnClick
        public static void OnClick(this ButtonWrapper wrapper, Action? callback) {
            wrapper.VisualElement.RegisterCallback<ClickEvent>( evt => callback?.Invoke() );
        }
        public static void OnClick(this ButtonWrapper wrapper, Action<ClickEvent>? callback) {
            wrapper.VisualElement.RegisterCallback<ClickEvent>( evt => callback?.Invoke( evt ) );
        }

        // OnChange
        public static void OnChange(this TextFieldWrapper<string> wrapper, Action<string?>? callback) {
            wrapper.VisualElement.RegisterCallback<ChangeEvent<string>>( evt => callback?.Invoke( evt.newValue ) );
        }
        public static void OnChange(this TextFieldWrapper<string> wrapper, Action<string?, string?>? callback) {
            wrapper.VisualElement.RegisterCallback<ChangeEvent<string>>( evt => callback?.Invoke( evt.newValue, evt.previousValue ) );
        }

        // OnChange
        public static void OnChange<T>(this PopupFieldWrapper<T> wrapper, Action<T?>? callback) where T : notnull {
            wrapper.VisualElement.RegisterCallback<ChangeEvent<T>>( evt => callback?.Invoke( evt.newValue ) );
        }
        public static void OnChange<T>(this PopupFieldWrapper<T> wrapper, Action<T?, T?>? callback) where T : notnull {
            wrapper.VisualElement.RegisterCallback<ChangeEvent<T>>( evt => callback?.Invoke( evt.newValue, evt.previousValue ) );
        }

        // OnChange
        public static void OnChange<T>(this SliderFieldWrapper<T> wrapper, Action<T>? callback) where T : struct, IComparable<T> {
            wrapper.VisualElement.RegisterCallback<ChangeEvent<T>>( evt => callback?.Invoke( evt.newValue ) );
        }
        public static void OnChange<T>(this SliderFieldWrapper<T> wrapper, Action<T, T>? callback) where T : struct, IComparable<T> {
            wrapper.VisualElement.RegisterCallback<ChangeEvent<T>>( evt => callback?.Invoke( evt.newValue, evt.previousValue ) );
        }

        // OnChange
        public static void OnChange(this ToggleFieldWrapper<bool> wrapper, Action<bool>? callback) {
            wrapper.VisualElement.RegisterCallback<ChangeEvent<bool>>( evt => callback?.Invoke( evt.newValue ) );
        }
        public static void OnChange(this ToggleFieldWrapper<bool> wrapper, Action<bool, bool>? callback) {
            wrapper.VisualElement.RegisterCallback<ChangeEvent<bool>>( evt => callback?.Invoke( evt.newValue, evt.previousValue ) );
        }

        // OnSubmit
        public static void OnSubmit(this VisualElementWrapper wrapper, Action? callback) {
            wrapper.VisualElement.RegisterCallback<NavigationSubmitEvent>( evt => callback?.Invoke() );
        }
        public static void OnSubmit(this VisualElementWrapper wrapper, Action<NavigationSubmitEvent>? callback) {
            wrapper.VisualElement.RegisterCallback<NavigationSubmitEvent>( evt => callback?.Invoke( evt ) );
        }

        // OnCancel
        public static void OnCancel(this VisualElementWrapper wrapper, Action? callback) {
            wrapper.VisualElement.RegisterCallback<NavigationCancelEvent>( evt => callback?.Invoke() );
        }
        public static void OnCancel(this VisualElementWrapper wrapper, Action<NavigationCancelEvent>? callback) {
            wrapper.VisualElement.RegisterCallback<NavigationCancelEvent>( evt => callback?.Invoke( evt ) );
        }

        // GetVisualElement
        public static VisualElement GetVisualElement(this VisualElementWrapper wrapper) {
            return wrapper.VisualElement;
        }
        public static T GetVisualElement<T>(this VisualElementWrapper<T> wrapper) where T : VisualElement {
            return wrapper.VisualElement;
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
            Assert.Object.Message( $"TextWrapper {this} is invalid" ).Valid( this is TextFieldWrapper<string> );
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
            Assert.Object.Message( $"ToggleWrapper {this} is invalid" ).Valid( this is ToggleFieldWrapper<bool> );
        }

    }
    // Slot
    public class SlotWrapper : VisualElementWrapper<VisualElement> {

        public IReadOnlyList<VisualElement> Children {
            get => (IReadOnlyList<VisualElement>) VisualElement.Children();
        }
        public event Action<VisualElement>? OnAddedEvent;
        public event Action<VisualElement>? OnRemovedEvent;

        public SlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Add(VisualElement element) {
            VisualElement.Add( element );
            OnAddedEvent?.Invoke( element );
        }
        public void Remove(VisualElement element) {
            VisualElement.Remove( element );
            OnRemovedEvent?.Invoke( element );
        }
        public bool Contains(VisualElement element) {
            return VisualElement.Contains( element );
        }

        public void OnAdded(Action<VisualElement>? callback) {
            OnAddedEvent += callback;
        }
        public void OnRemoved(Action<VisualElement>? callback) {
            OnRemovedEvent += callback;
        }

    }
}
