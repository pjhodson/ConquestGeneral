private var ThisHex : GameObject;
var GridPosition : Vector3;
var IsSelected : boolean;
function Start(){
ThisHex = gameObject;
}

function OnMouseEnter(){
ThisHex.renderer.material.color = Color(1,.92,.016,.7); //Yellow
}

function OnMouseDown(){
ThisHex.renderer.material.color = Color(0,0.7,.01);
}

function OnMouseExit(){
	if(!IsSelected){
		ThisHex.renderer.material.color = Color.white;
	}else{
		ThisHex.renderer.material.color = Color(0,0.7,.01); //Blue
	}
}