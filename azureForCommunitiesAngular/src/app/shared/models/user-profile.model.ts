import { Admission } from "./admission.model";
import { Placement } from "./placement.model";

export class UserProfile {

    public id?:string;
    public nickName?:string;
    public name?:string;
    public surname?:string;
    public email?:string;
    public phoneNumber?:string;
    public socialMedia?:string;
    public enterprise?:string;
    public mission?:string;
    public role?:string;

    public admissions?:Array<Admission>;
    public pendingAdmissions?:Array<Admission>;
    //public placements?:Array<Placement>;
}