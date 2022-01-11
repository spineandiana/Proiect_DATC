package com.example.datc;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.SearchView;
import androidx.core.app.ActivityCompat;

import android.Manifest;
import android.content.DialogInterface;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.graphics.Point;
import android.location.Address;
import android.location.Geocoder;
import android.location.Location;
import android.os.Bundle;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.Projection;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.Circle;
import com.google.android.gms.maps.model.CircleOptions;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;

import java.io.IOException;
import java.util.List;

public class MainActivity extends AppCompatActivity implements DialogInterface.OnClickListener, OnMapReadyCallback {

    //Initialize variable
    SupportMapFragment supportMapFragment;
    FusedLocationProviderClient client;
    TextView tv_normal, tv_satellite;
    GoogleMap Map;
    LatLng latLng;
    SearchView searchView;

    float[] distance = new float[2];

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //Assign variable
        supportMapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.google_map);
        searchView = findViewById(R.id.searchView);
        tv_normal = findViewById(R.id.tv_normal);
        tv_satellite = findViewById(R.id.tv_satellite);

        //Initialize fused client
        client = LocationServices.getFusedLocationProviderClient(this);

        tv_normal.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View view)
            {
                Map.setMapType(GoogleMap.MAP_TYPE_NORMAL);
                tv_normal.setBackgroundResource(R.drawable.map);
                tv_satellite.setVisibility(View.VISIBLE);
                tv_normal.setVisibility(View.GONE);
            }
        });

        tv_satellite.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View view)
            {
                Map.setMapType(GoogleMap.MAP_TYPE_HYBRID);
                tv_satellite.setBackgroundResource(R.drawable.satellite);
                tv_normal.setVisibility(View.VISIBLE);
                tv_satellite.setVisibility(View.GONE);
            }
        });


        //Check permission
        if (ActivityCompat.checkSelfPermission(MainActivity.this, Manifest.permission.ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED)
        {
            //when permission granted
            //call method
            getCurrentLocation();
        }
        else
        {
            //when permission denied
            //request permission
            ActivityCompat.requestPermissions(MainActivity.this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, 44);
        }

        searchView.setOnQueryTextListener(new SearchView.OnQueryTextListener()
        {
            @Override
            public boolean onQueryTextSubmit(String query)
            {
                String location = searchView.getQuery().toString();
                List<Address> addressList = null;
                if(location != null || !location.equals(""))
                {
                    Geocoder geocoder = new Geocoder(MainActivity.this);
                    try
                    {
                        addressList = geocoder.getFromLocationName(location, 1);
                    }
                    catch (IOException e)
                    {
                        e.printStackTrace();
                    }
                    Address address = addressList.get(0);
                    LatLng latLng = new LatLng(address.getLatitude(), address.getLongitude());
                    Map.addMarker(new MarkerOptions().position(latLng).title("You are here"));
                    Map.animateCamera(CameraUpdateFactory.newLatLngZoom(latLng, 10));
                }
                return false;
            }

            @Override
            public boolean onQueryTextChange(String newText) {
                return false;
            }
        });

        supportMapFragment.getMapAsync(this);
    }

    private void getCurrentLocation() {
        //Initialize task location
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            return;
        }
        Task<Location> task = client.getLastLocation();
        task.addOnSuccessListener(new OnSuccessListener<Location>() {
            @Override
            public void onSuccess(Location location)
            {
                if(location != null)
                {
                    //Sync map
                    supportMapFragment.getMapAsync(new OnMapReadyCallback()
                    {
                        @Override
                        public void onMapReady(GoogleMap googleMap)
                        {
                            //Initialize latitude and longitude
                            LatLng latLng = new LatLng(location.getLatitude(), location.getLongitude());
                            //Create marker options
                            MarkerOptions options = new MarkerOptions().position(latLng)
                                    .title("You are here")
                                    .draggable(true);
                            //Zoom map
                            googleMap.animateCamera(CameraUpdateFactory.newLatLngZoom(latLng, 10));
                            //Add marker on map
                            googleMap.addMarker(options);

                            Projection projection = googleMap.getProjection();
                            LatLng markerLocation = options.getPosition();
                            Point screenPosition = projection.toScreenLocation(markerLocation);

                            CircleOptions circleOptions = new CircleOptions();
                            circleOptions
                                    .center(new LatLng(37.42207410043655, -122.08198148588129))
                                    .radius(100)
                                    .strokeWidth(2)
                                    .strokeColor(Color.RED)
                                    .fillColor(Color.argb(42, 223, 29, 29));
                            googleMap.addCircle(circleOptions);

                            CircleOptions circleOptions1 = new CircleOptions();
                            circleOptions1
                                    .center(new LatLng(37.419666982917164, -122.0872439799203))
                                    .radius(100)
                                    .strokeWidth(2)
                                    .strokeColor(Color.RED)
                                    .fillColor(Color.argb(42, 223, 29, 29));
                            googleMap.addCircle(circleOptions1);

                            CircleOptions circleOptions2 = new CircleOptions();
                            circleOptions2
                                    .center(new LatLng(37.399503803616994, -122.04531568883557))
                                    .radius(100)
                                    .strokeWidth(2)
                                    .strokeColor(Color.RED)
                                    .fillColor(Color.argb(42, 223, 29, 29));
                            googleMap.addCircle(circleOptions2);

                            CircleOptions circleOptions3 = new CircleOptions();
                            circleOptions3
                                    .center(new LatLng(37.36362106701839, -122.03705448520357))
                                    .radius(100)
                                    .strokeWidth(2)
                                    .strokeColor(Color.RED)
                                    .fillColor(Color.argb(42, 223, 29, 29));
                            googleMap.addCircle(circleOptions3);


                            googleMap.setOnMapLongClickListener(new GoogleMap.OnMapLongClickListener()
                            {
                                @Override
                                public void onMapLongClick(LatLng latLng)
                                {
                                    /*Toast.makeText(MainActivity.this,
                                            "Lat: " + latLng.latitude + " , " +
                                            "Long: " + latLng.longitude,
                                            Toast.LENGTH_LONG).show();*/

                                    AlertDialog.Builder builder;
                                    builder = new AlertDialog.Builder(MainActivity.this);
                                    builder.setMessage("Add ambrosia zone?")
                                            .setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                                                public void onClick(DialogInterface dialogInterface, int id) {
                                                    //MarkerOptions options1 = new MarkerOptions().position(latLng);
                                                    //googleMap.addMarker(options1);
                                                    CircleOptions circleOptions4 = new CircleOptions();
                                                    circleOptions4
                                                            .center(new LatLng(latLng.latitude, latLng.longitude))
                                                            .radius(100)
                                                            .strokeWidth(2)
                                                            .strokeColor(Color.RED)
                                                            .fillColor(Color.argb(42, 223, 29, 29));
                                                    googleMap.addCircle(circleOptions4);
                                                    Toast.makeText(getApplicationContext(),"Ambrosia zone added on map", Toast.LENGTH_SHORT).show();
                                                }
                                            })
                                            .setNegativeButton("No", new DialogInterface.OnClickListener()
                                            {
                                                 public void onClick(DialogInterface dialog, int id) {
                                                 //  Action for 'NO' Button
                                                 dialog.cancel();
                                                 Toast.makeText(getApplicationContext(),"Action cancelled", Toast.LENGTH_SHORT).show();
                                                 }
                                            })
                                            .show();
                                }
                            });

                            googleMap.setOnMarkerDragListener(new GoogleMap.OnMarkerDragListener()
                            {
                                @Override
                                public void onMarkerDragStart(Marker marker)
                                {
                                    Log.d("System out", "onMarkerDragStart..."+marker.getPosition().latitude+"..."+marker.getPosition().longitude);
                                }

                                @Override
                                public void onMarkerDrag(Marker marker)
                                {
                                    Log.i("System out", "onMarkerDrag...");
                                }

                                @Override
                                public void onMarkerDragEnd(Marker marker)
                                {
                                    Log.d("System out", "onMarkerDragEnd..."+marker.getPosition().latitude+"..."+marker.getPosition().longitude);

                                    googleMap.animateCamera(CameraUpdateFactory.newLatLng(marker.getPosition()));

                                    //Location.distanceBetween(marker.getPosition().latitude, marker.getPosition().longitude, circleOptions1.getCenter().latitude, circleOptions1.getCenter().longitude, distance );
                                    //if (distance[0] < circleOptions1.getRadius())
                                    //{
                                        AlertDialog.Builder builder;
                                        builder = new AlertDialog.Builder(MainActivity.this);
                                        builder.setMessage("Ambrosia alert!!!")
                                                .setNegativeButton("OK", new DialogInterface.OnClickListener()
                                                {
                                                    public void onClick(DialogInterface dialog, int id)
                                                    {
                                                        //  Action for 'NO' Button
                                                        dialog.cancel();
                                                        //Toast.makeText(getApplicationContext(),"Action completed", Toast.LENGTH_SHORT).show();
                                                    }
                                                })
                                                .show();
                                    //}
                                }
                            });
                        }
                    });
                }
            }
        });
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults)
    {
        if(requestCode == 44)
        {
            if(grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED)
            {
                //when permission granted
                //call method
                getCurrentLocation();
            }
        }
    }

    @Override
    public void onClick(DialogInterface dialogInterface, int i)
    {
        Map.addCircle(new CircleOptions()
                .center(new LatLng(latLng.latitude, latLng.longitude))
                .radius(500)
                .strokeColor(Color.RED)
                .fillColor(Color.argb(42, 223, 29, 29)));
        Toast.makeText(MainActivity.this,
                "Ambrosia zone added on map", Toast.LENGTH_LONG).show();
    }

    @Override
    public void onMapReady(GoogleMap googleMap)
    {
        Map = googleMap;
    }
}