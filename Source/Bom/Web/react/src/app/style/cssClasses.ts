// typesafe access to shared css in reacdt

// macro.scc (relativly unspecific css classes)
type VariousMacrco = 'nowrap' | 'print' | 'screen' | 'submitDisguise' | 'print' | 'bold' | 'lineThrough' | 'w100perc';

type TableMacro = 'tableP0' | 'tableP2' | 'table5';

type PaddingMacro = 'p0' | 'p1' | 'p2' | 'p5' | 'p10' | 'ml10' | 'pl10';

type MarginMacro = 'm0' | 'm1' | 'm2' | 'm5' | 'm10';

type TextMacro = 'right' | 'left' | 'center' | 'vt' | 'vm' | 'vb';

type PosMacro = 'dib' | 'fr' | 'fl' | 'clear' | 'vm' | 'vb';

type BorderMacro = 'solidBox' | 'dottedBox';

export type Macro = VariousMacrco | TableMacro | PaddingMacro | MarginMacro | TextMacro | PosMacro | BorderMacro;

// layout.css (classes used in main layout but not anywhere else)
type ResponsiveLayout = 'mobileOnly' | 'tabletAndSmaller' | 'tabletOnly' | 'tabletAndBigger' | 'desktopOnly';

type OtherLayout = 'mainContent' | 'topBar' | 'title';

type Layout = ResponsiveLayout | OtherLayout;

// mainClasses.css  (classes having a specific layout)
type CoreMain = 'centerBox' | 'lightBox' | 'infoText' | 'infoText';

type MsgMain = 'messageArea' | 'infoMsg' | 'errorMsg';

type OtherMain = 'fbButton';

type Main = CoreMain | OtherMain | MsgMain;

// combine it all
export type AllClasses = Macro | Layout | Main;

export function get(cassClass: AllClasses): string {
    return cassClass;
}

export default get;
