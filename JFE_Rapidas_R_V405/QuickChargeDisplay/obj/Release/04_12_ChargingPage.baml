   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:QuickChargeDisplay.ControlQuickChargeDisplay.Control  K*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  %    QuickChargeDisplay.ChargingPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     <control(clr-namespace:QuickChargeDisplay.Control     @	Converter*clr-namespace:QuickChargeDisplay.Converter     2local clr-namespace:QuickChargeDisplay     -     N� Title$  ChargingPage��$	��800��$	��480�� � 	Resources ;   5QuickChargeDisplay.Converter.DoublePrecisionConverter   doublePrecisionConverter��%   &	           N� 
Background ��  �� ImageSource$0 *image\RAPIDAS-R_Monitor\05\충전중11.png��.��� .+�(   "QuickChargeDisplay.ExtendedBinding     Source ��  �� Path$ ChargingSoc�   QuickChargeDisplay.QCDV   Charging �� Source# Z    Target �� $ 
SocPercent�	 �� RelativeSource	 �� *FindAncestor+
 �� AncestorType"
 N�.   (QuickChargeDisplay.Control.AnimatedImage  -     Name$ 	BackImage��.   (QuickChargeDisplay.Control.AnimatedImage   	MouseDown$ BackImage_MouseDown��$��3��$��2���� $E��?image\04_충전중화면\배터리화면\자동차아이콘.png��$
��Fill=� K� ZIndex$ 3��$��180,78,500,281q��� -    ��Name$	 p10��$.��(image\RAPIDAS-R_Monitor\05\충전바.png��$#��84.378,55.398,682.622,261.602q��� -   $	 p20��$.��(image\RAPIDAS-R_Monitor\05\충전바.png��$$��113.986,55.398,651.014,261.602q��� -   $	 p30��$.��(image\RAPIDAS-R_Monitor\05\충전바.png��$$��144.496,55.398,620.504,261.602q��� -   $	 p40��$.��(image\RAPIDAS-R_Monitor\05\충전바.png��$$��174.842,55.398,590.158,261.602q��� -   $	 p50��$.��(image\RAPIDAS-R_Monitor\05\충전바.png��$$��205.352,55.398,559.648,261.602q��� -   $	 p60��$.��(image\RAPIDAS-R_Monitor\05\충전바.png��$$��234.764,55.398,530.236,261.602q��� -	   $	 p70��$.��(image\RAPIDAS-R_Monitor\05\충전바.png��$$��265.274,55.398,499.726,261.602q��� -
   $	 p80��$.��(image\RAPIDAS-R_Monitor\05\충전바.png��$$��295.784,55.398,469.216,261.602q��� -   $	 p90��$.��(image\RAPIDAS-R_Monitor\05\충전바.png��$$��326.294,55.398,438.706,261.602q��� -   $
 p100��$.��(image\RAPIDAS-R_Monitor\05\충전바.png��$��357,55,408,262q��� -    ��Name$ ChargeDevice��
������ $	 100�� �� Left$	 307��
 �� Top$	 268��$��71,42,350,250q��� -   $��SetPower��$��0��$��79��$
��Left=�$��144,290.872,0,0q�$	��Top=�$	��120�����	#FFFBF8F8$��50(�$��Right=����� �� -   $��	Txttimemm��$��0��$��50(����	#FFFBF8F8$��79��$
��Left=�$��Right=�$��603,290.308,0,0q�$	��Top=�$��89������ �� -   $��chargingSoc��$	��0.0��$��60(����	#FF0000E1$��97��$
��Left=�$��Right=�$��603,159.872,0,0q�$	��Top=�$	��111������ �� -    ��Name$
 Info��$��BMS Info��
���   �$��15(�$��0,0,371.746,368q� J� RenderTransformOrigin$ 0.486,0.963&�$
��Bold"�y��� *	Visible+ �� Mode$ TwoWay=�   BmsInfo# Z  �� UpdateSourceTrigger$ PropertyChanged=�.����  �� Orientation$ Vertical=�$��10,10,10,10q�.+��� $ 
Horizontal=�.+��� $��13(�$��74.233�� �� TextAlignment$ Right=�$
��Left=��� $�
Output V :���� -   $��OutputVoltage��$��13(�$��80��$��Right=����� $ 
OutVoltage�# Z $ TwoWay=�$ PropertyChanged=��� $��13(�$	��120��$ Right=��� $�
Output A :���� -   $��OutputCurrent��$��13(�$��97.003������ $ 	OutAmpare�# Z $ TwoWay=�$ PropertyChanged=��� $��2��$ 
Horizontal=�
��������� $ 
Horizontal=�.+��� $��13(�$��74.233��$ Right=�$
��Left=��� $�
Target V :���� -   $��TargetVoltage��$��13(�$��80��$��Right=����� $ TargetVoltage�# Z $ TwoWay=�$ PropertyChanged=��� $��13(�$	��120��$ Right=��� $�
Target A :���� -   $��TargetCurrent��$��13(�$��97.003������ $ TargetAmpare�# Z $ TwoWay=�$ PropertyChanged=��� $��2��$ 
Horizontal=�
��������� $ 
Horizontal=�.+��� $��13(�$��85��$ Right=�$
��Left=��� $�Remain Time :���� -    ��Name$ 	ResidTime��$��13(�$��75��$��Right=����� $ 
RemainTime�# Z $ TwoWay=�$ PropertyChanged=��� $��13(�$	��115��$ Right=��� $�SOC :���� -   $��	chargeSoc��$��13(�$��92������ $	 SOC�# Z $ TwoWay=�$ PropertyChanged=��� $��2��$ 
Horizontal=�
�������