import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { MonedaService, MonedaDTO } from '../../../services/moneda.service';

@Component({
  selector: 'app-moneda-detail',
  templateUrl: './moneda-detail.component.html',
  styleUrls: ['./moneda-detail.component.css']
})
export class MonedaDetailComponent implements OnInit {
  item?: MonedaDTO;
  loading = false;
  error: string | null = null;

  constructor(
    private monedaService: MonedaService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) {
        this.loadItem(id);
      }
    });
  }

  loadItem(id: number): void {
    this.loading = true;
    this.error = null;

    this.monedaService.getById(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading moneda:', error);
        this.error = error.message || 'Error al cargar la moneda';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
