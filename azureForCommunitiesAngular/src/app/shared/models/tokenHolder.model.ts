export class TokenHolder
{
    public token : string;
    public id : string;
    public expiration: Date;

    /**
     *
     */
    constructor(token?: string, id?:string, expiration?:Date) {
        this.token = token || "";
        this.id = id || "visitor";
        this.expiration = expiration || Object.create(null);
    }
}