import * as pdfMake from 'pdfmake/build/pdfmake';
import * as pdfFonts from 'pdfmake/build/vfs_fonts';
(<any>pdfMake).vfs = pdfFonts.pdfMake.vfs;

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Response } from 'src/app/Shared/Models/response.model';
import { Inventario } from '../../Shared/Models/inventario.model';



@Injectable({
  providedIn: 'root'
})
export class PneumaticiService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  getLast(): Observable<Response> {
    return this.http.get<Response>(environment.pneumatici + '/GetLast');
  }

  getPneumatici(targa: string): Observable<Response> {
    return this.http.get<Response>(environment.pneumatici + '/GetByTarga?targa=' + targa);
  }

  addPneumatici(pneumatici: Inventario): Observable<Response> {
    return this.http.post<Response>(environment.pneumatici + '/Post', pneumatici);
  }

  editPneumatici(pneumatici: Inventario): Observable<Response> {
    return this.http.put<Response>(environment.pneumatici + '/Put', pneumatici);
  }

  fineDeposito(pneumatici: Inventario): Observable<Response> {
    return this.http.put<Response>(environment.pneumatici + '/End', pneumatici);
  }

  inizioDeposito(pneumatici: Inventario): Observable<Response> {
    return this.http.put<Response>(environment.pneumatici + '/Start', pneumatici);
  }

  deactivatePneumatici(id: number): Observable<Response> {
    return this.http.delete<Response>(environment.pneumatici + '/Deactivate/' + id);
  }

  generatePdf(inventario: Inventario) {
    var residenza = '';
    var viaResidenza = '';
    var codiceFiscale = '';
    var partitaIVA = '';
    var telefono = '';
    var email = '';
    var quantita = ' ';
    var marca = ' ';
    var modello = ' ';
    var misura = ' ';
    var battistrada = ' ';
    var dot = ' ';
    var inizioDeposito = ' ';
    var statoGomme = ' ';
    if (inventario.pneumatici && inventario.pneumatici.veicolo && inventario.pneumatici.veicolo.cliente) {

      if (inventario.pneumatici.veicolo.cliente.comune) {
        residenza = ', Residente a ' + inventario.pneumatici.veicolo.cliente.comune;
        if (inventario.pneumatici.veicolo.cliente.provincia) {
          residenza += ', ' + inventario.pneumatici.veicolo.cliente.provincia;
        }
        if (inventario.pneumatici.veicolo.cliente.cap) {
          residenza += ' ' + inventario.pneumatici.veicolo.cliente.cap;
        }
      }
      if (inventario.pneumatici.veicolo.cliente.indirizzo) {
        viaResidenza = 'in via ' + inventario.pneumatici.veicolo.cliente.indirizzo;
        if (inventario.pneumatici.veicolo.cliente.civico) {
          viaResidenza += ', N ' + inventario.pneumatici.veicolo.cliente.civico;
        }
      }
      if (inventario.pneumatici.veicolo.cliente.codiceFiscale) {
        codiceFiscale += 'Codice Fiscale: ' + inventario.pneumatici.veicolo.cliente.codiceFiscale;
      }
      if (inventario.pneumatici.veicolo.cliente.codiceFiscale) {
        partitaIVA += 'Partita IVA: ' + inventario.pneumatici.veicolo.cliente.partitaIva;
      }
      if (inventario.pneumatici.veicolo.cliente.telefono) {
        telefono += 'Telefono: ' + inventario.pneumatici.veicolo.cliente.telefono;
      }
      if (inventario.pneumatici.veicolo.cliente.email) {
        email += 'Email: ' + inventario.pneumatici.veicolo.cliente.email;
      }
      if (inventario.pneumatici.quantita) { quantita = inventario.pneumatici.quantita.toString(); }
      if (inventario.pneumatici.marca) { marca = inventario.pneumatici.marca; }
      if (inventario.pneumatici.modello) { modello = inventario.pneumatici.modello; }
      if (inventario.pneumatici.misura) { misura = inventario.pneumatici.misura; }
      if (inventario.battistrada) { battistrada = inventario.battistrada + ' mm'; }
      if (inventario.pneumatici.dot) { dot = inventario.pneumatici.dot; }
      if (inventario.inizioDeposito) { inizioDeposito = inventario.inizioDeposito; }
      if (inventario.statoGomme) { statoGomme = inventario.statoGomme; }

      if (inventario.pneumatici.veicolo.targa && inventario.deposito && inventario.deposito.sede) {
        const documentDefinition: any = {
          content: [
            {
              text: inventario.deposito.sede.comune + ', ' + new Date().toLocaleDateString(),
            },
            {
              image: 'data:image/png;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAUDBAgICAgICAgICAgICAgICAgHBwgICAgICAgICAgICAgIChwLCAgOCQgGDSENDh0RHx8fBwsgICAeIBweHx4BBQUFBwYHDggIBRIKDgoVEh4SEhISEhISEhUSEhIaEhISEhISEhISFRIeEhUSEhISEhISEhUVEhISEhISEhISEv/AABEIASwBLAMBIgACEQEDEQH/xAAdAAEAAQUBAQEAAAAAAAAAAAAABwECBQYIBAMJ/8QAVhAAAQMDAgMEBQQIEQoHAAAAAAECAwQFEQYSBxMhCCIxURRBYXGRFSNSgSQyM0JicnOxFhg0NlNUVXR1gpKUs7TB0vAnN2WDk5WhtdPUCRcmQ2Nksv/EABQBAQAAAAAAAAAAAAAAAAAAAAD/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwDssAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACgAUCvTzT4oBUFnMb9Jv8pCvMb9JvxQC4FvMb9JPig5jfpJ8UAuBbzG/ST4oOY36SfFALgW8xv0k+KBHIvgqfEC4FCoAFFKb080+KAXAt3t80+I3t80+IFwLd7fNPiN7fNPiBcC3enmnxQcxvmnxQC4Fu9vmnxQrlPMCoKIVAAAAAAAAAAAAAAAAAAAD41MbnJhr3Rr5tRqr8Hpg1+6WWtciq29VtOietlPbVRP8Aa06mzFFQCJ7lbatFXGva2nXydR6ec1PqdQ5/4ms3SO7szyuJkDMftm12RfDz2QoTpW0UcjVR0UUi46c1jVT61xnBpF90c+VV2WjT86eVQ2VHL78Q4AhysuOpWZ28U7B7OdbLYz4qjVNfueq9XRZ5fE3Scn4y2mH/APdN0JGvvDiodnGidJVP4z9uf5UJplz4aXDrjhlpmRPOK8clfzJgDR6/iRryPKt11pSZE/YrjYMrj8F1N4mrXXj1runznU9slx+1ksk3w2U/U2bUfDmrbnmcOrXTp5w6phb8E5+SNdRaP5SOzpmGlXzbqZkiJ9SvA9K9qDW/7tr/ALrtf/bm38Gu1HqH5boGXu5elWyaZKepY6ioYNiTJy45kkgiRyIyRzHL7Gqc932iWF/3JkSeGxlUyox71apjk/x7gP2QhejkRyKioqIqKnVFReqKnsVOv1n0IR7G/Eb5e09FHNJvr7WraOqyuXvjRv2NOvscxFb/AKlSbVUCIu1VxSdpexvnpnNS5VcjaagR7UejZF78szmL0cxsaP8AH1q044/TVa1/dOH/AHbRf9M+PbE4i/L+opmQyI+htivoqTaqKx7mu+yJ2qnij5ETC+TEIVAnq3dpzW0y4S70UfqzPS0EKfF0fgbVauM+uajqmqNMxfvius0P/B8eSBdL2jnbfsOmnXznu8FKi+9sj0RCUdOaKqn45emrBUfldUWvr781QEk0HEXWr1+c13oqLP8ApC1yL8GQmx0GqdUyIiu4kaQb4dI4qSZevuYiGr2HQVf0/wDRGjX+2e908nx5Mq5N1s3Di4uVFTRXD+P2+lzPd8WQLkDI2656gf8AdOJFhcn/ANe2UX53KbRbPlB+Obr+J/h+p7faE8fLfEpdY+Htwjwr9P6MhT1pTrWqqe7NLgkCwWBIsc2itcX70Y5ce7fGgGFs1uqXqiN1XWVS/RSms6Z92ykybbb6CdmOZWTzY/ZI6Zuffyo0MgyNrejURE8kRERPghegBAAAAAAAAAAAAAAAAAAAAAAAACyaNHIrXJlF6KmcZT6i8Aare9BWuqR3No2yqqL0dU1MaKvllknQjK/8G7erlWPSdvqvLdqW4wuX6lbhCd1LXNyip59OiqnwVPADkrUfCVrcpFw3ik/Cp9Y1X5nx9SPb5wjrnblZw6roU+kzUkcqJ7e+n5zsbUnDu3Vu50/yj61+xr3dad38VtPUIir7CEtdcL9PN34tOuatyZT7FkuVQi+51TMqKByxqvh7VUyOc+w1VFjx33ekkx/FxlSNJmK1ytVMKiqioqouPrTop0Hqzh5Y41cv6GOIEK/SnSjj+PNgcpDOr7ZBA/FPS3SnbnwufLV2P9VEiZAkLsj8RP0P6ip1lkVtDcMUVYiqu1qPcnInx63Mk2p7pXnaXay4jfoe07USQyI2urkdRUW12HNkkbiWdvT/ANqNVd9aH5mMVUXKZRU8FTxRfUuU9fgbxxS4m3DUMdrjrV6Wuhjoo8OcqSKzotQ9F8ZnMRjVX8BANHcufHr7/wDHnn4nvslrqKmRGwQ85yL9qrmtRffvch5KSB8j2sYxZHuVEaxqKquXyREJg0hw3qHo11To+/1aKiKq0bJWIqeaZaoH20xwvvciNcmlqaqauOr7lRx5+NVkkzTnCK5OxzNAWiT8tftnxWF6l2nOFtkerUqOHmrWJhMvlrMtz+Kza5CYdFcHdKrtRlgvVDJ6vSa28RNT3vZU7EA1uz8GnorXScPdORr0y5NVV6Kn1JSuz7iSdLcMqGNU5unrdSefo13q6jHu3RtybTZdAW6jVFgSsbjwR13ucrf5Mk6obXGzaiNTOETCZVVX4r1UDFWrTtHS4WCFI8YxiSV3h+M/BlwAAAAAAAAAAAAAAAAAABQCoMXqO/0VugdU11VDSQMTLpaiRrGpjr0yuXL7EyRlVdpHSjXK2KrqqtEXCyUFsrKiL6nsiwoEwgj7RnGXTl3mbTUlziSqcmW0lWySkqXerux1LU3L7G5MvpDiFZ7vUVlJb62OpqLe/l1kTGSNdC9JHRYdvaiL84x6dM/aqBtQNLtXFKwVVXX0UNzp3VNsZK+uY7dGlMyCVsEzpJJGozDZHMb0VfE1eftF6T57KaG4vrHvkZGjqGknqIkdI5GNzIjNuMuTqBLgNUi4hWd94fYW10a3aNu99Htk5iN5DajO7bs+4ua7x9ZtQFQYvVN/pLZST19dM2npKZqPmmejlaxquaxFVGple85qdPMxlDry0z2lb5FWRvtbYZ51q0a/YkVM90cztqt3917Hp4eoDZyxyePq8uq9DSW8W9OfJrLu67UrLdJJJFHUSudGkkkTtr2RxubzJFRendRfE1WXtJaXa5USa4vZ+yx2eudCqeaP5OMe0Ddr/o6Wtzvu92p8/udVeh492xFwaBfezbZ69V9OuWoazPj6XepJs+9FZ1N20LxU0/e3LHbbnTVEyJl1PuWKobn1LDKiOV3ReiZ8FMhr/XdqsMEdTdqxlHBLLyI3yNkcjpdjpNiJG1Vzsa9fqAh79KBpHyuP8+X+6V/SgaR8rj/Pnf3CTdX8WtPWlIvTrnBHJPG2WGBiPmqZY3tR7HNp4WrImWuaveRPE1RnaT0rnElRXwMz1lqbRWxQon0le6LCN9oGvJ2QNJeVx/nzv7pm7B2cbRb1RaC5ahosftK+VECfBqYJL0brO13mHn2uvpq2NPF1PIjlbhcd5i99vXp1RDNVEzI2ue9zWMaiuc57ka1rU6qrnO6NRPNQMLpmwSUSIz0+tq2Ywvp83pEi+S81e8Z5E9/xUi2/doDSlJI6FbolTMxytfFbqeorHsVOnXkRqnw8jx0XaP0m9yNlrqiiz0R9xt1ZSx59Sb3xYQCXwYu036krKZKujqI6unVivbJTPbKjkRu7Ddq/bY9S48SIqjtTaQje6OSrq2SRucx7H0Tkcx7V2uY5M9HIqKn1ATiDVuGmvbbqKj9Otc6zQJK+F29ux7JGL1R7F6tymFT3n04ja4t2n6F1wucywU7ZGRIrWK975ZMq1jGJ1c5URy/xVA2Q12u13ZYJHwzXi0wyxOcySKa6Ukckb2rhzHsfJuY5F9SkXx9qjR73IxtZVOc5zWtRtE9VVVciIiJ61VVQ5d4w6c0zPf7zPUamko55rnWSy0r9OLK6nkklV74llbP31a5ypu9gH6CWO+UVcx0lFV0tZGx21z6OqiqGNdjO1zonKjXYx0Mic99iS22uktFx+Tbp8pQurt8s7rf6By3thYm1WK9dybURdy+ZsGtu0rpK1SOgfcHVc0blY+O2Q+kq1zVwrVeqoxV9yqBMgOb07ZOlM45N68cZ9BpfzelG26M7SukrnI2FlwfSTPcjGR3KBadXOcuEaj0yxFz5qgExgsiejkRzVRWqiKiouUVF8FRU6KntKyPRqZcqNTzVURPioFwLWuRUyi5TzTqmPZgqgFQAALHr0+CfFUT+0vLXJn8/1p1T+wDjfTsC6/1/c47s50tn08+ZkFtV7mwK6nnWlarmove3zNe9VXqqKieB19a7bT0saRU0ENPE1O7HTRMhYiJ4YYxMIcpao07ddB6uqtS0NDUXOwXZ0jq+OhYslTTOndzJt0adctm3yI5e7h2FVCaNP8f9IVkaSNvlHCuO9FWOdSSxr62vZOid5F6dPIDY+JHD2132mfBXUsT5FT5mqaxGVdNKn3OaCoZ85G5r9q9F9RzV2DqGSlverqWWV00tNPHTySvcrnSyQ1tTG+Vyr1VznIq59pMOpuPFskR1Jp7mahusnzdPTWyGSSCOR3dbLV1it5MEDXKiqufUQ52CvSflrVvpmPS+dF6XtVFb6T6bUekbVToqczd4Aabww0RT6g4jago61XvoYay5VVXTJI9kdY2KsbyYZtq96JJ1ikx/8aHcFt07QUkXLpqKjp2NbhG09LFEiYT8BpyZ2X/85erfddP+YwnZD0yi+1AOSrWv+Wus/eK/8kgOtjkfi1HUaW4iw6tqqWoms1ZA2Geqp4nStpc0jKKRZdqdxWoxr+vjnoTbScetHyR8xuoLcjcZxJI6J6e+OREei+wDzdrT9Zd//ebP61ARboBf8jsy/wChL7/XKot7SvFyK+afulDp6CouFIkLZLpdvR3wW+mpY5o1cyKWoRPSal0nJ7rM9NxdoH/M5N/Ad9/rlSB5+wvw2oJ7NFe6+JtbUOnqYqGOrzPBQRRyI2V1PBJ3GSyyJuV6JnuodVtiaibUREbjG1OjceGMeGDjXsicTJ9P2KFt3o6lthqamqdRXalgdUxUsrXo2qhro4UWSGNZcKj8eZ0PHx00i6PmpqG2bcZ61CNf/s17+fYBHHbG4Y0TrRPqCgiZQXe07aplXRJ6PJJG1+ZEk5WN0iJ1R/j9REPaJ1vNfuHemrjU9al13SGpend5s1NRVUUkmE6JuVM4QkHjnxOl1jTO03o+mqrl6Y9kdbc0gkgt8ELXZfHzpmoiqvrVcdE6ZU1TtcaJi09oXTtqY7mLTXViTyt6NlnfQVLp5Govg1XqvRfIDoXgLwyt9nttLMsTKm51EEE1ZcqlvOq5pXxMcjUmly6ONjVYxGtwncQkuqpY5WuZKxsrHJhzJWo9i+xWu6KQHww41R2uho7dq6Oaz1sNNTsirZoHyW2viWFjoZYauFFYyXlrGitdjGDd7lx40hBHzH6gtytxnEUqzvXHlHCivVfYBAfahsbdEXm0apsKJQJVVToLhSU3cpqjanMe3kJ3EjfCkiK3wRcKnUyHa01VW3e46c0rSTvpaa8Np6qt2Ows0dSqcqFyp4xtYk3dXoqq3PgeXXTLhxOvNuhoqKqpdL22bnTXCvhdA2sy5N7qdjky/dGisRG5+6KrsG49qnhTcah9pv8Ap+PmXCxbGrSNX5yemhVrouVn7Z7Ea9FanjzFwBNHD/QdqsVLFSW2igp2RtRqyNjb6RKv3z5p8b5HquV6+ZmbxaKWsidDV08FTE5FR0dTCyZiovRe69MESaJ7SGnqqNkd0nfYri1qekUN3glpnRv8HbJXt2OjV2cZwvmhltQdoLSNIxXfLVNUvx3IKBH1c0rl+1YxkKKm5Vwnex4gQXQxLofiNSWu2veyyX5kL5KFz3PhgdOsrEWJqrnfHJE1Ud5SKnqNY7eHCf5PrU1DRRKlJcHtbXNjb3IK5U+69E7jJkT+UjvMkHh1pW7aw1hHrC40M9rtVuaxlspq1nLqajk8zk7oV6tbukker18e7jJ0jrnS9LeLfVW2tZzKerhdDJ9Ju5O7IxfvZGuw5F9gHDXYK4hJbb2+01D9tLeGbWblRGsroWq6JyqvhujR8fvc09Xb74iLX3eGyQP+x7U1XVCNcuHV8yJua7zWKNGY/KOIT13pmv0tfJKSRdlVbqpk1POiYbIkT+ZS1UaL4tcjWu+pT5aOsNw1Pe4qVjnz1txqXPlmd3lTe5Xz1Eq+prU3rlfJEAm/sK8KPlO4LfqyLdQ2yVqUrX/az1yYc1yJ98yFFa73qiERdoj9dWov4auX9Op+mvD3SlJZLZR2yibtgpImxtXCI6R6rulmeiffvernL+Mp+ZXaI/XVqL+Grl/WFAnvsxWKvuegNUUNserK2oqVZDiTlK/uROfEkn3m9iPbn8I5g1Rpm4WyZ0FwoqijlRV+aqoHxr45XaiphU9qeZ1D2VF1CzRt3l00tOtyhuzZEgqImSJUwNgTmQxrIqNbNlWqir9FTUL/ANpHWdNOkF6oqNWNkTn0dwsrYUmY1cPZmTqiKme832AX6a496c9B9CumibRIzlclZbbFTwSqmNu/mTMWZJF6rnKeJfpqHhLWOY2Vbzb3yOxtqnvdEzK+Dp4+6jU818jIag4v8NbhAvpGj6iKoc3vrRQ0VM5XqiZVs8cyOxn75Uyc609qmuFXJHa6KqlR8rlgpoGSVcscauXlserEy5Ubt7y+SgfqvwwoaGmtNDBbKp1bQRQ7aSodUJU8yHKqzEyfbNai7U8kaiGldreqpodK10lW6vbAktFvW11MVNVJmqjRuyWZqta3OMpjwyYTsy6Yr9JaSldd0fzWLVXJ1G17XOpokjRUgznYkjtqvwnRFkUj7jbxbo9W8P73V0lNU0rKatt1O5tW6FXOc6qieit5TlTGPMDGcc+LVZa9LWS32qG7U6VdDQVLbvLURukSJ7qhH0ck0TcOq15bV3Nx4nQHZ713NfbUyaa31lA+mWGkX056PfU7KWKRapi7UVWOVy9Vz6yA+P8AA9/C/TcjGOeyGe3SSuYm5GR8qqbvcqfas3K1Mr9JDoHs/wCvLbfbPTyW6R720cVNQ1CSROjVlTFSxOexMph6Ijk7yASKAABRUKgCEuKF41jZ702upKRL5puRvz1vpGMZX0vzW2R2Xfdmo5qvTGc78e0w9Rxk0bL1rbFcY5vWys0mr5UXy3NhVF+J0IqFFb7/AIgc8V3GGSenkpdHaVuk1VIxzWTz2xlroYNyK1JVyic5G5ztXHghsPZa4SVGmqOrnuMrJ7tdJ1qa10XeZFlVckLXqnzjt6verunV6oTNj/C9S4DyQW+COR0rIYmSPzukZFG17srldz2t3OyvmeoqAPlUQMkarHta9q+LXta5q+9rkwphXaMtCu3ra7cr/prQUqu9+eWZ8AeRtvgSLkpFEkKpjlJExI8eXL27PUnqNF7QFCiaSv8ABTxeNormxxQRet0fgyOJviqqvRPMkRSgEE9iG3Pj0dSw1EL43elXDdHPE5iqi1GU3RyN6pjzJZk0ZaHO3utduc/x3ut9Krs+1eX4meCAeeioooW7Yoo4m/RijZG3p+CxMHNX/iG2+eosNtbBDLM5Ly1ypDFJKqN9AqU3KkbVVEyqdTp4oqe/6gNf0zb4prXQRVELJGpRUiLHPE16IqU0WUVkrcIuc9C+DRtpjdvjtlvY/wAdzKCla76lSPoZ1CoHziiaxEa1qNanREaiI1Pc1OievwLsFwAxtysVFU/qikpp/wAvTQy/0jFU+FDpa2wLmC30UKp64aKmjX4tjyZkAWomC4ADw1lqppnb5aeGR2ETdJBE92E8E3PbnHiKO1U0Lt8VPBG7CpujgiY7C+KbmNzg9wAoY+ayUb3Oe6lp3Oequc51NCrnKvirlVmVX2qZEAeaiooYUVsMUcbVXKpFGxiKvmqMbhVPlcLRS1H3engn/LwxS/0jVPcANZ/8v7Hu3fJFu3efoUPnnw24MvbrPS0/6npoIPV8xBFF098bUU94Aslia5qtc1HNcio5rmorVReioqL0VF9pj2WChbE+BtHSpA9Uc+FKWFInub4OdGjNrlTp1UyYA8rqCFYuQsUXJ27eSsTOVt+jy9u3HswW22209M1WU8EMDXO3ubBDHE1XYRNytjaiK7CImfYewAAAAAAAAAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//Z',
              alignment: 'center',
              height: 150,
              width: 150
            },
            /*{
              text: 'DICHIARAZIONE DI CONTO DEPOSITO PNEUMATICI',
              fontSize: 16,
              alignment: 'center',
              bold: true,
              style: 'sectionHeader'
            },
            { text: ' ' },*/
            { text: 'Il sottoscritto ' + inventario.pneumatici.veicolo.cliente.nome + residenza },
            { text: viaResidenza },
            {
              columns: [
                [
                  { text: codiceFiscale },
                  { text: telefono },
                ],
                [
                  {
                    text: partitaIVA,
                    alignment: 'right'
                  },
                  {
                    text: email,
                    alignment: 'right'
                  },
                ]
              ]
            },
            { text: ' ' },
            {
              text: 'DICHIARA',
              fontSize: 16,
              alignment: 'center',
              bold: true,
              style: 'sectionHeader'
            },
            {
              text: 'Di aver dato conto deposito a GRUPPOVIS Spa, a titolo gratuito i seguenti pneumatici usati: ',
            },
            { text: ' ' },
            { text: ' ' },
            {
              columns: [
                [
                  { text: 'Quantità:' },
                  { text: 'Marca:' },
                  { text: 'Modello:' },
                  { text: 'Misura:' },
                  { text: 'Battistrada:' },
                  { text: 'DOT:' },
                  { text: 'Data Ubicazione:' },
                  { text: 'Stato delle gomme:' }
                ],
                [
                  {
                    text: quantita,
                    alignment: 'center'
                  },
                  {
                    text: marca,
                    alignment: 'center'
                  },
                  {
                    text: modello,
                    alignment: 'center'
                  },
                  {
                    text: misura,
                    alignment: 'center'
                  },
                  {
                    text: battistrada,
                    alignment: 'center'
                  },
                  {
                    text: dot,
                    alignment: 'center'
                  },
                  {
                    text: inizioDeposito,
                    alignment: 'center'
                  },
                  {
                    text: statoGomme,
                    alignment: 'center'
                  }
                ]
              ]
            },
            { text: ' ' },
            { text: ' ' },
            {
              text: 'FIRMA',
              fontSize: 16,
              alignment: 'center',
              bold: true,
              style: 'sectionHeader'
            },
            {
              text: '(                    in fede                    )',
              alignment: 'right'
            },
            {
              text: ' ',
              alignment: 'right'
            },
            {
              text: ' ',
              alignment: 'right'
            },
            {
              text: '______________________________',
              alignment: 'right'
            },
            {
              text: ' ',
              alignment: 'right'
            },
            {
              text: '(firma leggibile)            ',
              alignment: 'right'
            },
            {
              text: ' ',
            },
            {
              text: ' ',
            },
            {
              text: ' Autorizzo il trattamento dei miei dati personali ai sensi del Decreto Legislativo 30 giugno 2003, n. 196 e del GDPR (Regolamento UE 2016/679).'
            },
            {
              text: ' ',
              alignment: 'right'
            },
            {
              text: '(                    in fede                    )',
              alignment: 'right'
            },
            {
              text: ' ',
              alignment: 'right'
            },
            {
              text: ' ',
              alignment: 'right'
            },
            {
              text: '______________________________',
              alignment: 'right'
            },
            {
              text: ' ',
              alignment: 'right'
            },
            {
              text: '(firma leggibile)            ',
              alignment: 'right'
            },
          ],
          styles: {
            sectionHeader: {
              bold: true,
              decoration: 'underline',
              fontSize: 14,
              margin: [0, 15, 0, 15]
            }
          }
        };
        pdfMake.createPdf(documentDefinition).download(inventario.pneumatici.veicolo.targa);
        //var win = window.open(inventario.pneumatici.veicolo.targa, '_blank');
        if (window.innerWidth > 415) { pdfMake.createPdf(documentDefinition).open({}, window); }
      }
    }
  }
}