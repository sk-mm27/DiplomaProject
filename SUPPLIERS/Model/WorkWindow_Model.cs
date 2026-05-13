using SUPPLIERS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SUPPLIERS.Model
{
    public class Rating
    {
        public double RATING { get; set; }
        public string NAME { get; set; }
        public string INN { get; set; }
        public string TYPE_ACTIVITY { get; set; }
        public string PROFILE { get; set; }
        public int DeliveryOnTime { get; set; }
        public int DeliveryNotOnTime { get; set; }
    }

    class WorkWindow_Model
    {
        public List<Rating> RatingSupliers()
        {
            List<Rating> rating = new List<Rating>();
            PurchaseInvoice_Model pim = new PurchaseInvoice_Model();
            foreach (SUPPLIER s in new Supplier_Model().GetAllSupplier())
            {
                int dot = pim.DeliveryOnTime(s.SUPPLIER_ID);
                int dont = pim.DeliveryNotOnTime(s.SUPPLIER_ID);

                rating.Add(new Rating
                {
                    RATING = dot - dont,
                    NAME = s.NAME,
                    INN = s.INN,
                    TYPE_ACTIVITY = s.TYPE_ACTIVITY.NAME,
                    PROFILE = s.PROFILE.NAME,
                    DeliveryOnTime = dot,
                    DeliveryNotOnTime = dont
                });
            }

            return rating.OrderByDescending(ra => ra.RATING).ToList();
        }

        public string GetRegistryOnZakupki(string Inn)
            => "https://zakupki.gov.ru/epz/dishonestsupplier/search/results.html?searchString=" + Inn +
                "&morphology=on&search-filter=Дате+размещения&sortBy=UPDATE_DATE&pageNumber=1&sortDirection=false&recordsPerPage=_10&showLotsInfoHidden=false&fz94=on&fz223=on&ppRf615=on";


        public string GetAddressOnMap(string Address)
            => "https://www.google.com/maps/search/" + Address.Replace('^', ' ');
    }
}
