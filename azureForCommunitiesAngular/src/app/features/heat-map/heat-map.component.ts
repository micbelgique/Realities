import { Input } from '@angular/core';
import { Component, OnInit, SimpleChanges } from '@angular/core';
import { Loader } from '@googlemaps/js-api-loader';
import { Anchor } from 'src/app/shared/models/anchor-model';
import * as mapStyles from  "src/app/_files/map-style.json"
import { LocationModel } from 'src/app/shared/models/location.model';
import { AnchorsMapService } from 'src/app/shared/services/anchors-map.service';

@Component({
  selector: 'app-heat-map',
  templateUrl: './heat-map.component.html',
  styleUrls: ['./heat-map.component.css']
})
export class HeatMapComponent implements OnInit {

  private map?: google.maps.Map;

  @Input() userPosition?:LocationModel;
  @Input() anchors?:Array<Anchor>;

  private iconBase:string = "http://maps.google.com/mapfiles/kml/paddle/";

  private isDataWaiting:boolean = false;

  constructor(public mapService:AnchorsMapService) { }

  ngOnInit(): void {
        
    let loader = new Loader({
      apiKey: 'AIzaSyAxzS4IA4H5bvIzqeT_KEp6uCoJ6IJK6mM',
      libraries: ["places","visualization"]
    });

    loader.load().then( () => {
      
      let mapStyle:google.maps.MapTypeStyle[] = mapStyles.silver as google.maps.MapTypeStyle[];

      this.map = new google.maps.Map(document.getElementById("map") || Object.create(null), {
        center: {lat: 51.23, lng: 6.78},
        zoom: 13,
        disableDefaultUI: true,
        fullscreenControl: true,
        styles: mapStyle
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
        let heatMapData = [];
          for (let anchor of this.anchors) {
            heatMapData.push(new google.maps.LatLng(anchor.location!.latitude, anchor.location!.longitude));
          }
          let heatmap = new google.maps.visualization.HeatmapLayer({
            data: heatMapData
          });
          heatmap.setMap(this.map!);
      }
  }
}
