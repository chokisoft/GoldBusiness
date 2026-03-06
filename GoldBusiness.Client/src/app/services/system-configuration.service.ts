import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

// Modelos de localización: incluir 'descripcion' que devuelve el backend.
// Mantener 'nombre' opcional para compatibilidad con otras partes que lo usen.
export interface Pais { id: number; descripcion?: string; nombre?: string; }
export interface Provincia { id: number; descripcion?: string; nombre?: string; paisId: number; }
export interface Municipio { id: number; descripcion?: string; nombre?: string; provinciaId: number; }
export interface CodigoPostal { id: number; codigo: string; municipioId: number; }

export interface SystemConfigurationDTO {
  id?: number;
  codigoSistema: string;
  licencia: string;
  nombreNegocio: string;
  direccion?: string;
  paisId: number;
  provinciaId: number;
  municipioId: number;
  codigoPostalId: number;
  // Propiedades de presentación que la UI usa
  municipio?: string;
  provincia?: string;
  codPostal?: string;

  imagen?: string;
  web?: string;
  email?: string;
  telefono?: string;
  cuentaPagarId?: number;
  cuentaCobrarId?: number;
  caducidad: string;
  creadoPor?: string;
  fechaHoraCreado?: string;
  modificadoPor?: string;
  fechaHoraModificado?: string;
  cuentaPagarCodigo?: string;
  cuentaPagarDescripcion?: string;
  cuentaCobrarCodigo?: string;
  cuentaCobrarDescripcion?: string;
  estaVigente?: boolean;
  estaVencida?: boolean;
  proximoAVencer?: boolean;
  diasRestantes?: number;
  estadoLicencia?: string;
  tieneCuentasConfiguradas?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class SystemConfigurationService {
  private readonly endpoint = 'SystemConfiguration';

  constructor(private apiService: ApiService) { }

  getAll(): Observable<SystemConfigurationDTO[]> {
    return this.apiService.get<SystemConfigurationDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<SystemConfigurationDTO> {
    return this.apiService.get<SystemConfigurationDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: SystemConfigurationDTO): Observable<SystemConfigurationDTO> {
    return this.apiService.post<SystemConfigurationDTO>(this.endpoint, dto);
  }

  update(id: number, dto: SystemConfigurationDTO): Observable<SystemConfigurationDTO> {
    return this.apiService.put<SystemConfigurationDTO>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }

  // Métodos para selects dependientes (rutas coherentes con WebApi controllers)
  getPaises(): Observable<Pais[]> {
    return this.apiService.get<Pais[]>('pais');
  }

  // Corregido: coincide con ProvinciaController -> GET api/provincia/pais/{paisId}
  getProvinciasByPais(paisId: number): Observable<Provincia[]> {
    return this.apiService.get<Provincia[]>(`provincia/pais/${paisId}`);
  }

  getMunicipiosByProvincia(provinciaId: number): Observable<Municipio[]> {
    return this.apiService.get<Municipio[]>(`municipio/provincia/${provinciaId}`);
  }

  getCodigosPostalesByMunicipio(municipioId: number): Observable<CodigoPostal[]> {
    return this.apiService.get<CodigoPostal[]>(`codigopostal/by-municipio/${municipioId}`);
  }

  /**
   * Sube el archivo de logo al servidor.
   * Devuelve el nombre del archivo guardado en disco.
   */
  uploadLogo(codigoSistema: string, file: File): Observable<{ fileName: string }> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('codigoSistema', codigoSistema);
    return this.apiService.postFormData<{ fileName: string }>(`${this.endpoint}/upload-logo`, formData);
  }

  /**
   * Devuelve la URL completa del logo para usar en [src] de <img>.
   */
  getLogoUrl(fileName: string): string {
    if (!fileName) return '';
    if (fileName.startsWith('http://') || fileName.startsWith('https://')) {
      return fileName;
    }

    const anyApi: any = this.apiService as any;
    if (typeof anyApi.buildUrl === 'function') {
      return anyApi.buildUrl(`${this.endpoint}/logo/${fileName}`);
    }
    return `/api/${this.endpoint}/logo/${fileName}`;
  }
}
