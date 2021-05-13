export class LocationModel {

    public latitude:number;
    public longitude:number;
    public srid:number;

    constructor(lat?:number, lng?:number, srid?:number) {
        this.latitude = lat || 0;
        this.longitude = lng || 0;
        this.srid = srid || 4326;
    }
}