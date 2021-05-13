import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { Loader } from '@googlemaps/js-api-loader';
import { Anchor } from 'src/app/shared/models/anchor-model';
import { LocationModel } from 'src/app/shared/models/location.model';
import { AnchorsMapService } from 'src/app/shared/services/anchors-map.service';

@Component({
  selector: 'app-anchors-map',
  templateUrl: './anchors-map.component.html',
  styleUrls: ['./anchors-map.component.css']
})
export class AnchorsMapComponent implements OnInit {

  private map?: google.maps.Map;

  @Input() userPosition?:LocationModel;
  @Input() anchors?:Array<Anchor>;

  private iconBase:string = "http://maps.google.com/mapfiles/kml/paddle/";

  private isDataWaiting:boolean = false;

  constructor(public mapService:AnchorsMapService) { }

  ngOnInit(): void {
    
    let loader = new Loader({
      apiKey: 'AIzaSyAxzS4IA4H5bvIzqeT_KEp6uCoJ6IJK6mM'
    });

    loader.load().then( () => {
      this.map = new google.maps.Map(document.getElementById("map") || Object.create(null), {
        center: {lat: 0, lng: 0},
        zoom: 14  
      });

      if(this.isDataWaiting)
        this.initMap(this.userPosition || new LocationModel(50.45857741638403, 3.9413667667414143));
    }).catch( () => {
      console.log("erreur de google");
    });

    loader.apiKey
  }

  ngOnChanges(changements: SimpleChanges) {
    if (typeof google === 'object' && typeof google.maps === 'object')
      this.initMap(this.userPosition || new LocationModel(50.45857741638403, 3.9413667667414143));
    else
      this.isDataWaiting = true;

  }

  initMap(position:LocationModel)
  {
      this.map?.panTo({lat: position.latitude, lng: position.longitude})
      if(this.anchors != null)
      {
        for (let anchor of this.anchors) {

          let anchorIcon;
          switch (anchor.visibility!) {
            case "Public":
              anchorIcon = this.iconBase + "grn-blank.png";
              break;
            case "Private":
              anchorIcon = this.iconBase + "red-blank.png";
              break;
            case "Group":
              anchorIcon = this.iconBase + "wht-blank.png"
              break;

            default:
              anchorIcon = this.iconBase + "grn-blank.png";
              break;
          }

          console.log(anchorIcon);

          const marker = new google.maps.Marker({
            position: new google.maps.LatLng(anchor.location!.latitude, anchor.location!.longitude),
            label: String(anchor.id),
            icon: anchorIcon,
            map: this.map,
          });
        }
      }
  }

}
