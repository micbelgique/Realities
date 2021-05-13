import { Admission } from "./admission.model";
import { EpiCenterModel } from "./epiCenter-model";
import { UserProfile } from "./user-profile.model";

export class CommunityModel {
    
    public id?: number;
    public name?:string;
    public address?: string;
    public infoUrl?: string;
    public pictureUrl?: string;
    public epiCenter?: EpiCenterModel;

    public admissions?:Array<Admission>;
    public pendingAdmissions?:Array<Admission>;

    public admissionsUsers?:Array<UserProfile>;
    public pendingAdmissionsUsers?:Array<UserProfile>;

    //public users?: Array<UserProfile>;
    public isLocated?:boolean;

    constructor() {

    }
}