import { LocationModel } from "./location.model";

export class Anchor {

    public userName?:string;

    public id?: Number;
    public identifier?: string;
    public model?: string;
    public address?: string;
    public pictureUrl?:string;
    public userId?:string;

    public location?:LocationModel;

    public creationDate?:Date;
    public lastUpdateDate?:Date;

    public visibility?:string;

    constructor() {
        
    }
}
