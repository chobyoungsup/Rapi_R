   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  ,    &QuickChargeDisplay.AdjustUnitPricePage   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    0local clr-namespace:QuickChargeDisplay    -     N� Title$  AdjustUnitPricePage��$
��1024��$	��768�� � 	Resources  �� 
TargetType   ButtonTextBlock �� Property �� Value  GridViewHeaderTextBlock��%�  &	        &	 �     �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  �@��TextAlignment$ Right=���  � |� .  �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �   ���  � ��
 ���� ��  �@��TextAlignment$ Right=���  � |� .   N� 
Background ��  �� ImageSource$8 2image\10_관리자화면\관리자기본배경.gif��.���� .+��� $<��6image\10_관리자화면\아이콘\단가조정_ON.gif�� �� Left$	 442��
 �� Top$ 24��$��50��$��48���� #��[  $��30(�$	 498��$ 30��단가조정�� -   	 ��Name$	 logOut��$4��.image\10_관리자화면\아이콘\LOGOUT1.gif��$	 954��$ 24��$��50��$��55���� -   $	 ChargeDetails��$=��7image\10_관리자화면\아이콘\충전내역_OFF.gif��$ 43��$	 107��$��44��$��51���� -   $	 TroubleDetails��$=��7image\10_관리자화면\아이콘\고장내역_OFF.gif��$ 46��$	 199��$��43��$��45���� -   $	 AdjustUnitPrice��$<��6image\10_관리자화면\아이콘\단가조정_ON.gif��$ 43��$	 286��$��45��$��50���� -   $	 SetupEquipment��$=��7image\10_관리자화면\아이콘\기기설정_OFF.gif��$ 43��$	 374��$��45��$��49���� -   $	 
PowerMeter��$=��7image\10_관리자화면\아이콘\파워메타_OFF.gif��$ 40��$	 465��$��48��$��61���� -   $	 EquipmentInfo��$=��7image\10_관리자화면\아이콘\기기정보_OFF.gif��$ 48��$	 569��$��40��$��60���� -	   $	 FactorySetup��$=��7image\10_관리자화면\아이콘\공장설정_OFF.gif��$ 44��$	 679��$��50��$��50���� $ 25��$	 162��#��[  충전내역�� $ 25��$	 250��#��[  고장내역�� $ 25��$	 341��#��[  단가조정�� $ 25��$	 429��#��[  기기설정�� $ 25��$	 532��#��[  파워메타�� $ 25��$	 635��#��[  기기정보�� $ 25��$	 735��#��[  공장설정�� -
   
 ��Name$
 UnitPriceList��$	 133��$	 135��$	��890��$	��632������  �� Path$
 List�   QuickChargeDisplay.QCDV   ChargeHistory �� Source# Z  �� View �� .P��� .���� #��[ NO �� CellTemplate �� .R�� $��80��$��30��.+���  �� TextAlignment$ Right=�$��15(�$��Right=�$��Center=����� $ Number��� .���� #��[ 	날 짜 �� .R��� $ Center=�$��15(�$	��150������ $
 Date��� .���� #��[ 	시 간 �� .R��� $ Center=�$��15(�$	��150������ $
 Time��� .���� #��[ 단가 정보 �� .R��� $
 Left=�$��15(�$	��500������ $ Details��� $ 715.929��$ 97.172��$��font\YoonYGO550 TT.ttf)�$
��Bold"�$��20(�$��34.296��$��90.027��.��날짜선택�� -    ��Name$ cbUnitPriceDate��$	 812��$ 98.601��$	��200��$��30.009��