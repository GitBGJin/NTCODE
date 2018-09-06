dhtmlXGridObject.prototype.splitAt_old = dhtmlXGridObject.prototype.splitAt;
dhtmlXGridObject.prototype.splitAt=function(ind){
	this.splitAt_old(ind);
	this.sync_scroll=this._fake.sync_scroll=function(end){
		var old=this.objBox.style.overflowX;
	    if (this.obj.offsetWidth<=this.objBox.offsetWidth)
        {
        	if (!end) return this._fake.sync_scroll(true);
            this.objBox.style.overflowX="hidden";
            this.objBox.style.overflowY="hidden";
            this._fake.objBox.style.overflowX="hidden";
        }
        else{
            this.objBox.style.overflowX="scroll";
            this.objBox.style.overflowY="auto";
            this._fake.objBox.style.overflowY="hidden";
            this._fake.objBox.style.overflowX="scroll";
        }
        return old!=this.objBox.style.overflowX;
    };
};