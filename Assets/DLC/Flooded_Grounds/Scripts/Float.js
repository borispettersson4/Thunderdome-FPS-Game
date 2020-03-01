#pragma strict


public var WaterHeight = 15.5;
//private var WaterHeight2 = 15.0;

//private var min = 1.1;
//private var max = 1.1;

function Start () {

	//WaterHeight2 = WaterHeight;
	//min = WaterHeight - 1;
	//max = WaterHeight + 1;

}

function Update () {


	//WaterHeight2 = WaterHeight + Mathf.PingPong(Time.time * 0.2, 0.2);

	if(this.transform.position.y < WaterHeight){
		//this.transform.position.y = Mathf.Lerp(WaterHeight, WaterHeight2, Time.time);
		this.transform.position.y = WaterHeight;
	}
	
	

}