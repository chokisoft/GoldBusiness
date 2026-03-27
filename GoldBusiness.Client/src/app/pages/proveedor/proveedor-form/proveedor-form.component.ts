import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, of } from 'rxjs';
import { switchMap, catchError } from 'rxjs/operators';
import { ProveedorDTO, ProveedorService } from '../../../services/proveedor.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';
import { CodigoPostalService, CodigoPostalDTO } from '../../../services/codigo-postal.service';
import { normalizePhone, phoneValidator, PHONE_MAX_LENGTH } from '../../shared/phone.util';

@Component({
  selector: 'app-proveedor-form',
  templateUrl: './proveedor-form.component.html',
  styleUrls: ['./proveedor-form.component.css']
})
export class ProveedorFormComponent implements OnInit, OnDestroy {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;

  paises: PaisDTO[] = [];
  provincias: ProvinciaDTO[] = [];
  municipios: MunicipioDTO[] = [];
  codigosPostales: CodigoPostalDTO[] = [];

  selectedPais?: PaisDTO;

  private subs: Subscription[] = [];

  constructor(
    private fb: FormBuilder,
    private proveedorService: ProveedorService,
    private paisService: PaisService,
    private provinciaService: ProvinciaService,
    private municipioService: MunicipioService,
    private codigoPostalService: CodigoPostalService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(5)]],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      nif: ['', Validators.maxLength(11)],
      iban: ['', Validators.maxLength(27)],
      bicoSwift: ['', Validators.maxLength(11)],
      iva: [0, [Validators.required, Validators.min(0), Validators.max(99.99), Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      direccion: ['', Validators.maxLength(256)],
      paisId: [null],
      provinciaId: [null],
      municipioId: [null],
      codigoPostalId: [null],
      web: ['', Validators.maxLength(256)],
      email1: ['', [Validators.email, Validators.maxLength(256)]],
      email2: ['', [Validators.email, Validators.maxLength(256)]],
      telefono1: ['', Validators.maxLength(PHONE_MAX_LENGTH)],
      telefono2: ['', Validators.maxLength(PHONE_MAX_LENGTH)],
      fax1: ['', Validators.maxLength(PHONE_MAX_LENGTH)],
      fax2: ['', Validators.maxLength(PHONE_MAX_LENGTH)],
      cancelado: [false]
    });

    // Inicialmente deshabilitar dependientes
    this.itemForm.get('provinciaId')!.disable();
    this.itemForm.get('municipioId')!.disable();
    this.itemForm.get('codigoPostalId')!.disable();
  }

  ngOnInit(): void {
    this.loadPaises();

    this.subs.push(
      this.itemForm.get('paisId')!.valueChanges.pipe(
        switchMap(v => {
          this.onPaisChange(v);
          if (v) {
            return this.paisService.getById(+v).pipe(
              catchError(() => of(null))
            );
          }
          return of(null);
        })
      ).subscribe((pais: PaisDTO | null) => {
        this.applyPhoneValidators(pais || undefined);
      })
    );

    this.subs.push(this.itemForm.get('provinciaId')!.valueChanges.subscribe(v => this.onProvinciaChange(v)));
    this.subs.push(this.itemForm.get('municipioId')!.valueChanges.subscribe(v => this.onMunicipioChange(v)));

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
      }
    });
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }

  private loadPaises(): void {
    this.paisService.getAll().subscribe({
      next: list => this.paises = list,
      error: err => console.error('Error loading paises', err)
    });
  }

  private loadProvincias(paisId?: number): void {
    this.provincias = [];
    this.municipios = [];
    this.codigosPostales = [];
    this.itemForm.patchValue({ provinciaId: null, municipioId: null, codigoPostalId: null }, { emitEvent: false });
    this.itemForm.get('municipioId')!.disable();
    this.itemForm.get('codigoPostalId')!.disable();

    if (!paisId) {
      this.itemForm.get('provinciaId')!.disable();
      return;
    }

    this.itemForm.get('provinciaId')!.enable();
    this.provinciaService.getByPaisId(paisId).subscribe({
      next: list => this.provincias = list,
      error: err => console.error('Error loading provincias', err)
    });
  }

  private loadMunicipios(provinciaId?: number): void {
    this.municipios = [];
    this.codigosPostales = [];
    this.itemForm.patchValue({ municipioId: null, codigoPostalId: null }, { emitEvent: false });
    this.itemForm.get('codigoPostalId')!.disable();

    if (!provinciaId) {
      this.itemForm.get('municipioId')!.disable();
      return;
    }

    this.itemForm.get('municipioId')!.enable();
    this.municipioService.getByProvinciaId(provinciaId).subscribe({
      next: list => this.municipios = list,
      error: err => console.error('Error loading municipios', err)
    });
  }

  private loadCodigoPostales(municipioId?: number): void {
    this.codigosPostales = [];
    this.itemForm.patchValue({ codigoPostalId: null }, { emitEvent: false });

    if (!municipioId) {
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }

    this.itemForm.get('codigoPostalId')!.enable();
    this.codigoPostalService.getByMunicipioId(municipioId).subscribe({
      next: list => this.codigosPostales = list,
      error: err => console.error('Error loading codigos postales', err)
    });
  }

  onPaisChange(value: number | null): void {
    const paisId = value ? +value : undefined;
    if (!paisId) {
      this.selectedPais = undefined;
      this.itemForm.patchValue({ provinciaId: null, municipioId: null, codigoPostalId: null }, { emitEvent: false });
      this.provincias = [];
      this.municipios = [];
      this.codigosPostales = [];
      this.itemForm.get('provinciaId')!.disable();
      this.itemForm.get('municipioId')!.disable();
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }
    this.loadProvincias(paisId);
  }

  onProvinciaChange(value: number | null): void {
    const provinciaId = value ? +value : undefined;
    if (!provinciaId) {
      this.itemForm.patchValue({ municipioId: null, codigoPostalId: null }, { emitEvent: false });
      this.municipios = [];
      this.codigosPostales = [];
      this.itemForm.get('municipioId')!.disable();
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }
    this.loadMunicipios(provinciaId);
  }

  onMunicipioChange(value: number | null): void {
    const municipioId = value ? +value : undefined;
    if (!municipioId) {
      this.itemForm.patchValue({ codigoPostalId: null }, { emitEvent: false });
      this.codigosPostales = [];
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }
    this.loadCodigoPostales(municipioId);
  }

  private applyPhoneValidators(pais?: PaisDTO): void {
    this.selectedPais = pais;

    const telefono1 = this.itemForm.get('telefono1')!;
    const telefono2 = this.itemForm.get('telefono2')!;
    const base = [Validators.maxLength(PHONE_MAX_LENGTH)];
    const validators = [...base];

    if (pais && pais.regexTelefono) {
      validators.push(phoneValidator(pais.regexTelefono));
    }

    telefono1.setValidators(validators);
    telefono2.setValidators(validators);

    telefono1.updateValueAndValidity({ emitEvent: false });
    telefono2.updateValueAndValidity({ emitEvent: false });
  }

  private loadItem(): void {
    if (!this.itemId) return;
    this.loading = true;
    this.error = null;

    this.proveedorService.getById(this.itemId).subscribe({
      next: item => {
        this.itemForm.patchValue({
          codigo: item.codigo,
          descripcion: item.descripcion,
          nif: item.nif,
          iban: item.iban,
          bicoSwift: item.bicoSwift,
          iva: item.iva,
          direccion: item.direccion,
          web: item.web,
          email1: item.email1,
          email2: item.email2,
          telefono1: item.telefono1,
          telefono2: item.telefono2,
          fax1: item.fax1,
          fax2: item.fax2,
          cancelado: item.cancelado
        }, { emitEvent: false });

        if (item.paisId) {
          this.itemForm.patchValue({ paisId: item.paisId }, { emitEvent: false });

          this.paisService.getById(item.paisId).subscribe({
            next: pais => this.applyPhoneValidators(pais),
            error: () => this.applyPhoneValidators(undefined)
          });

          this.provinciaService.getByPaisId(item.paisId).subscribe({
            next: provinces => {
              this.provincias = provinces;
              this.itemForm.get('provinciaId')!.enable();

              if (!item.provinciaId) {
                this.loading = false;
                return;
              }

              this.itemForm.patchValue({ provinciaId: item.provinciaId }, { emitEvent: false });

              this.municipioService.getByProvinciaId(item.provinciaId).subscribe({
                next: municipios => {
                  this.municipios = municipios;
                  this.itemForm.get('municipioId')!.enable();

                  if (!item.municipioId) {
                    this.loading = false;
                    return;
                  }

                  this.itemForm.patchValue({ municipioId: item.municipioId }, { emitEvent: false });

                  this.codigoPostalService.getByMunicipioId(item.municipioId).subscribe({
                    next: cps => {
                      this.codigosPostales = cps;
                      this.itemForm.get('codigoPostalId')!.enable();

                      if (item.codigoPostalId) {
                        this.itemForm.patchValue({ codigoPostalId: item.codigoPostalId }, { emitEvent: false });
                      }
                      this.loading = false;
                    },
                    error: err => {
                      console.error('Error loading cp', err);
                      this.loading = false;
                    }
                  });
                },
                error: err => {
                  console.error('Error loading municipios', err);
                  this.loading = false;
                }
              });
            },
            error: err => {
              console.error('Error loading provincias', err);
              this.loading = false;
            }
          });
        } else {
          this.applyPhoneValidators(undefined);
          this.loading = false;
        }
      },
      error: err => {
        this.error = err.message || 'Error al cargar el proveedor';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.itemForm.invalid) {
      this.itemForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.error = null;

    const raw = this.itemForm.getRawValue();
    const formData: ProveedorDTO = {
      ...raw,
      id: this.itemId,
      telefono1: normalizePhone(raw.telefono1),
      telefono2: normalizePhone(raw.telefono2),
      fax1: normalizePhone(raw.fax1),
      fax2: normalizePhone(raw.fax2)
    };

    const request = this.isEditMode
      ? this.proveedorService.update(this.itemId!, formData)
      : this.proveedorService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/nomencladores/proveedores']);
      },
      error: err => {
        this.error = err.message || 'Error al guardar el proveedor';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/nomencladores/proveedores']);
  }
}
