import java.sql.Array
import groovy.time.TimeDuration
import groovy.time.TimeCategory
import static Constants.*



class Constants {
    static int idxProjectes   = 0
    static int idxTasques     = 0
    static int idxImputacions = 0
    static String theInfoName = 'C:\\PROJECTES\\track_my_time.tasks'
    static int _PROJECTE     = 0
    static int _TASQUES      = 1
    static int _IMPUTACIONS_STARTED       = 2
    static int _IMPUTACIONS_DUE_COMPLETED = 3
    def taskCharacters = ["✔","☐"]
    def estats = [_PROJECTE, _TASQUES, _IMPUTACIONS_STARTED, _IMPUTACIONS_DUE_COMPLETED ]
}

class Projecte  {
    int id
    String desc
    def tasques

    Projecte (String _desc) {
        id = idxProjectes++
        desc = _desc
        tasques = [:]
    }

}

class Tasca {
    int id
    String desc
    def imputacions

    Tasca (String desc) {
        id = idxTasques++
        this.desc = desc
        imputacions = [:]
    }

}

class Imputacio {
    static def pattern = '^.*([0-9][0-9]/[0-9][0-9]/[0-9][0-9][0-9][0-9] +[0-9]+:[0-9]+:[0-9]+).*'
    Date start, stop;
    String sStart,sStop

    Tasca tasca

    private parse () {
        try {
            def s1 = (sStart =~ (pattern))[0][1]
            def s2 = (sStop =~ (pattern))[0][1]
            start = new Date ( s1 )
            stop  = new Date ( s2 )
        }
        catch ( Exception ex ) {
            throw new Exception( "Imputació sense inici o final " +  " TASCA [" + (tasca?.desc ?: "[tasca desconeguda]") + " ] "   +  sStart + " / " + sStop )
        }
    }


    boolean   filtre (Date ini, Date end) {
        parse()
        if ( start >= ini && stop<=end )
            return true
        else
            return false
    }

    TimeDuration  getDeltaTemps () {
        parse()
        TimeDuration duration = TimeCategory.minus(stop, start)
        return duration
    }
}







void main (Date start, Date stop) {
    def _A_projectes   = [:]   // PROJECETS : TASQUES

    //PARSE FILE
/*
    Cualquier tarea es la que tiene los caracteres ✔ o ☐
 */

    File theInfoFile = new File( theInfoName )


    def estat
    Imputacio imputacio


    theInfoFile.eachLine { line->
        def patternProject     = ~/^.+:/
        def patternTask        = ~/^.+[✔|☐].+/
        def patternImputacions        = ~/^.+> Imputacions.*/
        def patternTasquesStarted     = ~/^.+@started.*/
        def patternTasquesDue         = ~/^.+@due.*/
        def patternTasquesDone         = ~/^.+@done.*/


        /*
        print    line ==~ patternProject
        print    line ==~ patternTask
        print '|'
        print    line ==~ patternImputacions
        print   ' | ' + estat + ' | '
        println  " | " + line
        print '|'
         */


        // match

        // comencem orijecte
        if ( line ==~ patternProject ) {
            projecte = line
            _A_projectes.put ( projecte , new Projecte(line) )
        }

        if ( line ==~ patternTask ) {
            tasca = line
            _A_projectes[projecte].tasques.put (tasca,new Tasca(tasca))
            estat = _TASQUES
        }

        if ( line ==~ patternImputacions && ( estat == _TASQUES || estat == _IMPUTACIONS_STARTED )) {
            estat = _IMPUTACIONS_STARTED

        }


        if ( line ==~ patternTasquesStarted && (estat == _IMPUTACIONS_STARTED ) ) {
            imputacio = new Imputacio()
            imputacio.tasca = _A_projectes[projecte].tasques[tasca]
            imputacio.sStart = line
        }

        if ( (line ==~ patternTasquesDue || line ==~ patternTasquesDone )  && ( estat == _IMPUTACIONS_STARTED ) ) {
            imputacio.sStop = line
            _A_projectes[projecte].tasques[tasca].imputacions.put ( idxImputacions++, imputacio )
        }

    }



    // REPORT
    println "Imputacions per el period dhsdhdhsdkash " + start + " / " + stop
    TimeDuration totalImputacions = new TimeDuration(0,0,0,0)
    _A_projectes.each {
        TimeDuration totalImputacionsPerProjecte = new TimeDuration(0,0,0,0)
        it.value.each() {
            Projecte p = it
            println p.desc

            p.tasques.each() {
                Tasca t = it.value
                TimeDuration totalImputacionsPerTasca = new TimeDuration(0,0,0,0)
                if( t.imputacions != [:] ) {
                    String forquillestemps = ""
                    t.imputacions.each() {
                        Imputacio i = it.value
                        if ( i.filtre(start,stop)) {
                            try {
                                TimeDuration delta = i.deltaTemps
                                forquillestemps += "                 " + delta.toString() + "\n"
                                totalImputacions += delta
                                totalImputacionsPerProjecte += delta
                                totalImputacionsPerTasca += delta
                            }
                            catch (Exception ex) {
                                println "ERROR :" + ex.message
                            }
                        }
                    }

                    if ( forquillestemps != "" ) {
                        println t.desc
                        println forquillestemps
                        println "         TOTAL TASCA: " + totalImputacionsPerTasca
                    }
                }

            }
            println "\n------------------------------------------------------------"
            println "         TOTAL PROJECTE:" + totalImputacionsPerProjecte


        }
    }

    println "------------------------------------------------------------"
    println "     TOTAL IMPUTACIONS:" + totalImputacions
}


Date periode1 = new Date ("01/10/2016 00:00:00")
Date periode2 = new Date ("24/10/2016 23:59:59")

main (periode1, periode2)



