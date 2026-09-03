using SilkKey = Silk.NET.Input.Key;

namespace Nitor;

public static class Keyboard
{
	public static Key Escape { get; } = new(SilkKey.Escape);

	public static Key F1 { get; } = new(SilkKey.F1);
	public static Key F2 { get; } = new(SilkKey.F2);
	public static Key F3 { get; } = new(SilkKey.F3);
	public static Key F4 { get; } = new(SilkKey.F4);
	public static Key F5 { get; } = new(SilkKey.F5);
	public static Key F6 { get; } = new(SilkKey.F6);
	public static Key F7 { get; } = new(SilkKey.F7);
	public static Key F8 { get; } = new(SilkKey.F8);
	public static Key F9 { get; } = new(SilkKey.F9);
	public static Key F10 { get; } = new(SilkKey.F1);
	public static Key F11 { get; } = new(SilkKey.F1);
	public static Key F12 { get; } = new(SilkKey.F1);

	public static Key PrintScreen { get; } = new(SilkKey.PrintScreen);
	public static Key ScrollLock { get; } = new(SilkKey.ScrollLock);
	public static Key Pause { get; } = new(SilkKey.Pause);

	public static Key Insert { get; } = new(SilkKey.Insert);
	public static Key Home { get; } = new(SilkKey.Home);
	public static Key PageUp { get; } = new(SilkKey.PageUp);
	public static Key PageDown { get; } = new(SilkKey.PageDown);
	public static Key Delete { get; } = new(SilkKey.Delete);
	public static Key End { get; } = new(SilkKey.End);

	public static Key Number0 { get; } = new(SilkKey.Number0);
	public static Key Number1 { get; } = new(SilkKey.Number1);
	public static Key Number2 { get; } = new(SilkKey.Number2);
	public static Key Number3 { get; } = new(SilkKey.Number3);
	public static Key Number4 { get; } = new(SilkKey.Number4);
	public static Key Number5 { get; } = new(SilkKey.Number5);
	public static Key Number6 { get; } = new(SilkKey.Number6);
	public static Key Number7 { get; } = new(SilkKey.Number7);
	public static Key Number8 { get; } = new(SilkKey.Number8);
	public static Key Number9 { get; } = new(SilkKey.Number9);

	public static Key Q { get; } = new(SilkKey.Q);
	public static Key W { get; } = new(SilkKey.W);
	public static Key E { get; } = new(SilkKey.E);
	public static Key R { get; } = new(SilkKey.R);
	public static Key T { get; } = new(SilkKey.T);
	public static Key Y { get; } = new(SilkKey.Y);
	public static Key U { get; } = new(SilkKey.U);
	public static Key I { get; } = new(SilkKey.I);
	public static Key O { get; } = new(SilkKey.O);
	public static Key P { get; } = new(SilkKey.P);

	public static Key A { get; } = new(SilkKey.A);
	public static Key S { get; } = new(SilkKey.S);
	public static Key D { get; } = new(SilkKey.D);
	public static Key F { get; } = new(SilkKey.F);
	public static Key G { get; } = new(SilkKey.G);
	public static Key H { get; } = new(SilkKey.H);
	public static Key J { get; } = new(SilkKey.J);
	public static Key K { get; } = new(SilkKey.K);
	public static Key L { get; } = new(SilkKey.L);

	public static Key Z { get; } = new(SilkKey.Z);
	public static Key X { get; } = new(SilkKey.X);
	public static Key C { get; } = new(SilkKey.C);
	public static Key V { get; } = new(SilkKey.V);
	public static Key B { get; } = new(SilkKey.B);
	public static Key N { get; } = new(SilkKey.N);
	public static Key M { get; } = new(SilkKey.M);

	public static Key Backquote { get; } = new(SilkKey.GraveAccent);
	public static Key Minus { get; } = new(SilkKey.Minus);
	public static Key Equal { get; } = new(SilkKey.Equal);
	public static Key Backspace { get; } = new(SilkKey.Backspace);

	public static Key Tab { get; } = new(SilkKey.Tab);
	public static Key LeftBracket { get; } = new(SilkKey.LeftBracket);
	public static Key RightBracket { get; } = new(SilkKey.RightBracket);
	public static Key BackSlash { get; } = new(SilkKey.BackSlash);

	public static Key Semicolon { get; } = new(SilkKey.Semicolon);
	public static Key Apostrophe { get; } = new(SilkKey.Apostrophe);
	public static Key Enter { get; } = new(SilkKey.Enter);

	public static Key LeftShift { get; } = new(SilkKey.ShiftLeft);
	public static Key RightShift { get; } = new(SilkKey.ShiftRight);
	public static Key Comma { get; } = new(SilkKey.Comma);
	public static Key Period { get; } = new(SilkKey.Period);
	public static Key Slash { get; } = new(SilkKey.Slash);

	public static Key Space { get; } = new(SilkKey.Space);

	public static Key LeftControl { get; } = new(SilkKey.ControlLeft);
	public static Key RightControl { get; } = new(SilkKey.ControlRight);
	public static Key CapsLock { get; } = new(SilkKey.CapsLock);

	public static Key UpArrow { get; } = new(SilkKey.Up);
	public static Key DownArrow { get; } = new(SilkKey.Down);
	public static Key LeftArrow { get; } = new(SilkKey.Left);
	public static Key RightArrow { get; } = new(SilkKey.Right);

	public static Key Numpad { get; } = new(SilkKey.NumLock);
	public static Key KeypadAdd { get; } = new(SilkKey.KeypadAdd);
	public static Key KeypadSubtract { get; } = new(SilkKey.KeypadSubtract);
	public static Key KeypadMultiply { get; } = new(SilkKey.KeypadMultiply);
	public static Key KeypadDivide { get; } = new(SilkKey.KeypadDivide);
	public static Key KeypadEnter { get; } = new(SilkKey.KeypadEnter);

	public static Key Keypad0 { get; } = new(SilkKey.Keypad0);
	public static Key Keypad1 { get; } = new(SilkKey.Keypad1);
	public static Key Keypad2 { get; } = new(SilkKey.Keypad2);
	public static Key Keypad3 { get; } = new(SilkKey.Keypad3);
	public static Key Keypad4 { get; } = new(SilkKey.Keypad4);
	public static Key Keypad5 { get; } = new(SilkKey.Keypad5);
	public static Key Keypad6 { get; } = new(SilkKey.Keypad6);
	public static Key Keypad7 { get; } = new(SilkKey.Keypad7);
	public static Key Keypad8 { get; } = new(SilkKey.Keypad8);
	public static Key Keypad9 { get; } = new(SilkKey.Keypad9);
}
