import { Anchor } from "./anchor-model";
import { Interaction } from "./interaction.model";

export class FullAnchor extends Anchor {
    public interactions?:Array<Interaction>;
}